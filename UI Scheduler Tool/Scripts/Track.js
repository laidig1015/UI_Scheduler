﻿// NAMESPACE //
var Track = {};

// UTIL //
if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

// CONSTATNTS //
var MAX_SEMESTERS = 8;
var MAX_COURSES = 20;

// TRACK //
function TrackModel() {
    this.matrix = new Array(MAX_SEMESTERS);
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.matrix[i] = new CourseList();
    }
    this.nodes = {};
    this.totalSemesterHours = 0;
}

TrackModel.prototype.loadCurriculum = function (track, callback) {
    var self = this;
    $.get("/Track/GetCurriculumNodes", "trackName=" + track, function (nodes, status) {
        if (!nodes) {
            console.log("error getting nodes!");
            return false;
        }
        console.log("got nodes: %o", nodes);
        var numNodes = nodes.length;
        var regexFilter = /(<([^>]+)>)/ig;
        for (var i = 0; i < numNodes; i++) {
            var node = nodes[i];
            // always trust the db source for now
            self.matrix[node.index].add({ course: node.course, preqDirty: [], isOffered: true });
            if (!(node.id in self.nodes)) {
                self.nodes[node.course.id] = node;
                // filter any html tags that might be embeded in our description
                var cleanDesc = self.nodes[node.course.id].course.description;
                cleanDesc = cleanDesc.replace(regexFilter, "");
                self.nodes[node.course.id].course.description = cleanDesc;
            }
        }
        callback();
    }, "json").fail(function (err, status) {
        console.log("error getting nodes: %s (%s)", err, status);
    });
}

TrackModel.prototype.hasCourse = function (courseId) {
    for (var s = 0; s < MAX_SEMESTERS; s++) {
        if (this.matrix[s].hasCourse(courseId)) {
            return s;
        }
    }
    return -1;
}

TrackModel.prototype.hasTaken = function (startingFrom, courseId) {
    for (var s = startingFrom; s >= 0; s--) {
        if (this.matrix[s].hasCourse(courseId)) {
            return s;
        }
    }
    return -1;
}

TrackModel.prototype.willTake = function (startingFrom, courseId) {
    for (var s = startingFrom; s < MAX_SEMESTERS; s++) {
        if (this.matrix[s].hasCourse(courseId)) {
            return s;
        }
    }
    return -1;
}

TrackModel.isOfferedIn = function (semesterIndex, course) {
    var isSpring = semesterIndex & 1;// is odd and we assume we start in fall
    return (isSpring && course.isOfferedInSpring) || (!isSpring && course.isOfferedInFall);
}

TrackModel.prototype.checkPreqs = function (semesterIndex, node) {
    var dirty = new Array();
    var stack = new Array();
    var numInitialParents = node.parents.length;
    // start by pushing our initial nodes parents to the stack
    for (var i = 0; i < numInitialParents; i++) {
        stack.push({ node: this.nodes[node.parents[i]], index: semesterIndex });
    }

    while (stack.length > 0) {// while we have no more nodes to check
        var currFrame = stack.pop();// get the current stack frame we are looking at
        if (!currFrame.node) {// ignore if we don't have it in our hash
            continue;
        }
        // first check if we have taken this class from the semester we are stupposed to
        // start checking it from
        var takenAt = this.hasTaken(currFrame.index, currFrame.node.course.id);
        if (takenAt == -1) {
            // if we didnt' take it then mark it as dirty
            dirty.push(currFrame);
        } else {
            // if we did take it then add its parents to the stack to be checked
            // since we did find it all of the parents should start their search
            // from the index we found the class at
            var numParents = currFrame.node.parents.length;
            for (var i = 0; i < numParents; i++) {
                stack.push({ node: this.nodes[currFrame.node.parents[i]], index: takenAt });
            }
        }
    }
    return dirty;// return an array of our dirty nodes
}

TrackModel.prototype.add = function (semesterIndex, courseItem) {
    var result = { wasAdded: false, errors: [] };
    var course = courseItem.course;

    // find the current node
    var node = this.nodes[course.id];

    // make sure we haven't taken this course in the past
    var takenIn = this.hasCourse(course.id);
    if (takenIn != -1) {
        console.log("class already taken in: %d", takenIn);
        result.errors.push({ type: "ALREADY_EXISTS", takenIn: takenIn });
    }

    // update if this new item is validated for this semester
    courseItem.isOffered = TrackModel.isOfferedIn(semesterIndex, node.course);
    if (!courseItem.isOffered) {
        console.log("class not offered!");
    }

    // check if we have the required prequisties to take the course
    courseItem.preqDirty = this.checkPreqs(semesterIndex, node);
    if (courseItem.preqDirty.length > 0) {
        console.log("preq error when adding course, dirty %o", courseItem.preqDirty);
        result.errors.push({ type: "PREQ", dirty: courseItem.preqDirty });
    }

    this.matrix[semesterIndex].add(courseItem)
    result.wasAdded = true;
    return result;
}

TrackModel.prototype.remove = function (semesterIndex, courseId) {
    var result = { wasRemoved: false, errors: [], item: null };

    // make sure we have the course in this semester
    if (!this.matrix[semesterIndex].hasCourse(courseId)) {
        console.log("course not in matrix");
        result.errors.push({ type: "DOES_NOT_EXIST" });
        return result;
    }

    // check its immediate children for conflicts
    var node = this.nodes[courseId];
    var conflicts = new Array();
    var numChildren = node.children.length;
    for (var i = 0; i < numChildren; i++) {
        var child = this.nodes[node.children[i]];
        if (!child) {
            continue;// ignore if we don't have it in our hash
        }
        // if we have one of our children taken in the future don't let us remove
        // this from the matrix
        var takenAt = this.willTake(semesterIndex, child.course.id);
        if (takenAt > -1) {
            // invalidate this course in our matrix
            this.matrix[takenAt].courses[child.course.id].hasPreqErrors = true;
            conflicts.push({ node: child, index: takenAt });
        }
    }

    // if we found any conflicts then don't let us remove
    if (conflicts.length > 0) {
        console.log("conflicts with classes in the future: %o", conflicts);
        result.errors.push({ type: "PREQ_CONFLICTS", conflict: conflicts });
    }

    result.item = this.matrix[semesterIndex].remove(courseId);
    result.wasRemoved = true;
    return result;
}

// COURSE LIST //
function CourseList() {
    this.courses = {};
    this.numSemesterHours = 0;
    this.numCourses = 0;
    this.specialCourseCases = [];
}

CourseList.prototype.find = function (courseId) {
    return (this.hasCourse(courseId)) ? this.courses[courseId] : null;
}

CourseList.prototype.hasCourse = function (courseId) {
    return (courseId in this.courses);
}

CourseList.prototype.hasRoom = function (course) {
    return (this.numCourses < MAX_CLASSES);
}

CourseList.prototype.add = function (courseItem) {
    var course = courseItem.course;
    if (this.hasCourse(course.id)) {
        return;
    }
    this.courses[course.id] = courseItem;
    this.numCourses++;
    if (isNaN(course.hours)) {
        this.specialCourseCases.push(course);
    } else {
        this.numSemesterHours += parseInt(course.hours);
    }
}

CourseList.prototype.remove = function (courseId) {
    if (!this.hasCourse(courseId)) {
        return null;
    }

    var courseItem = this.courses[courseId];
    var course = courseItem.course;
    var specialIndex = this.specialCourseCases.indexOf(course);
    if (specialIndex > -1) {
        this.specialCourseCases.splice(specialIndex, 1);
    } else {
        this.numSemesterHours -= parseInt(course.hours);
    }
    this.numCourses--;
    delete this.courses[courseId];
    return courseItem;
}

// TRACK VIEW //
function TrackView(model) {
    this.semesters = [];
    this.model = model;
    this._uniqueId = 0;
    this._lastRemoved = null;
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.semesters.push(document.getElementById("course-list-" + i));
    }
    // add links to itself
    var courseLists = [];
    for (var i = 0; i < 8; i++) {
        courseLists.push('#course-list-' + i);
    }
    var courseSelector = courseLists.join(',');
    var self = this;
    $(courseSelector).sortable({
        connectWith: ".course-list",
        remove: function (event, ui) {
            var semesterIndex = parseInt(event.target.id.slice(-1), 10);
            var courseId = ui.item.context.id.split('-')[1];
            var result = self.model.remove(semesterIndex, courseId);
            self._lastRemoved = result.item;
        },
        receive: function (event, ui) {
            var semesterIndex = parseInt(event.target.id.slice(-1), 10);
            var courseId = ui.item.context.id.split('-')[1];
            var result = self.model.add(semesterIndex, self._lastRemoved);
        }
    }).disableSelection();
}

TrackView.prototype.clear = function (semesterIndex) {
    var list = this.semesters[semesterIndex];
    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }
}

TrackView.prototype.clearAll = function () {
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.clear(i);
    }
}

TrackView.createCourseElement = function (courseItem, newId) {
    // emulating this template
    //<li id='semester-#{courseId}' class='course-container'>
    //    <h3 class='course-name'>#{name} (#{courseId})</h3>
    //    <a id='course-toggle-info-#{uniqueId}' class='course-toggle info-toggle' href='#>Description</a>
    //    <p id='course-content-info-#{uniqueId}' class='course-content'>#{description}</p>
    //    <a id='course-toggle-error-#{uniqueId}' class='course-toggle error-toggle' href='#'>Error</a>
    //    <p id='course-content-error-#{uniqueId}' class='course-content'>#{errors}</p>
    //</li>

    // variables are:
    // 0: course id
    // 1: unique course item id
    // 2: course name
    // 3: course description
    // 4: error description
    var template = "<li id='semester-{0}' class='course-container'><h3 class='course-name'>{2} ({0})</h3><a id='course-toggle-info-{1}' class='course-toggle info-toggle' href='#'>Description</a><p id='course-content-info-{1}' class='course-content'>{3}</p><a id='course-toggle-error-{1}' class='course-toggle error-toggle' href='#'>Error</a><p id='course-content-error-{1}' class='course-content'>{4}</p></li>";
    var course = courseItem.course;
    var element = document.createElement('div');
    element.innerHTML = String.format(template, course.id, newId, course.name, course.description, "AN ERROR HAS OCCURED");
    return element;
}

TrackView.prototype.render = function () {
    var infoToggle, infoContent, errorToggle, errorContent;

    for (var s = 0; s < MAX_SEMESTERS; s++) {
        this.clear(s);
        var semester = this.model.matrix[s];
        for (var courseId in semester.courses) {
            var item = semester.courses[courseId];
            var newId = this._uniqueId++;
            var element = TrackView.createCourseElement(item, newId);
            while (element.children.length > 0) {
                this.semesters[s].appendChild(element.children[0]);
            }

            // DON'T FORGET YOUR SCOPING!!! SEE: http://stackoverflow.com/questions/8909652/adding-click-event-listeners-in-loop
            infoToggleName = '#course-toggle-info-' + newId;
            infoContentName = '#course-content-info-' + newId;
            if (typeof window.addEventListener === 'function') {
                (function (_infoContentName) {
                    $(infoToggleName).click(function () {
                        $(_infoContentName).slideToggle("fast");
                    });
                })(infoContentName);
            }

            errorToggleName = '#course-toggle-error-' + newId;
            errorContentName = '#course-content-error-' + newId;
            if (typeof window.addEventListener === 'function') {
                (function (_errorContentName) {
                    $(errorToggleName).click(function () {
                        $(_errorContentName).slideToggle("fast");
                    });
                })(errorContentName);
            }
        }
    }
}