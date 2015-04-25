var Preq = {};

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

var MAX_SEMESTERS = 8;
var MAX_CLASSES = 20;

function TrackMatrix() {
    this.matrix = new Array(MAX_SEMESTERS);
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.matrix[i] = [];
    }
    this.nodes = {};
    this.errors = new Array();
    this.coursePool = new Array();
    this.totalSemsterHours = 0;
}

TrackMatrix.prototype.loadSeed = function (track) {
    var self = this;
    $.get("/Track/GetCurriculumNodes", "trackName=" + track, function (nodes, status) {
        console.log("got nodes: %o", nodes);
        var numNodes = nodes.length;
        for (var i = 0; i < numNodes; i++) {
            var node = nodes[i];
            self.matrix[node.index].push(node.course);
            if (!(node.id in self.nodes)) {
                self.nodes[node.course.id] = node;
            }
        }
        self.renderSemesters();
        return true;
    }, "json").fail(function (err, status) {
        console.log("error getting nodes: %s (%s)", err, status);
    });
    return false;
}

TrackMatrix.prototype.addCourse = function (semester, courseId) {
    // make sure we have more room in our semester
    if (this.matrix[semester].length >= MAX_CLASSES) {
        console.log("max classes reached");
        return { wasSuccessful: false, errorType: "FULL" };
    }

    // first make sure we haven't taken this class already
    var takenIn = this.hasTakenCourse(courseId);
    if (takenIn != -1) {
        console.log("class already taken in semster %d", takenIn + 1);// offset from 0 index
        return { wasSuccessful: false, errorType: "TAKEN", takenIn: takenIn };
    }

    // find the current node
    var node = this.nodes[courseId];

    // first check if the course is offered in this semester
    if (!TrackMatrix.courseIsOfferedIn(semester, node.course)) {
        console.log("%s is not available in %s", courseId, (semester & 1 ? "spring" : "fall"));
        return { wasSuccessful: false, errorType: "OFFERING" };
    }

    // check if we have the required prequisties to take the course
    var dirty = this.checkPreq(semester, node);
    if (dirty.length > 0) {
        console.log("preq error when adding course, dirty %o", dirty);
        return { wasSuccessful: false, errorType: "PREQ", dirty: dirty };
    }

    // finally add it to our matrix
    var index = this.indexOfCourseInPool(courseId);
    var old = this.coursePool[index];
    this.coursePool.splice(index, 1);
    this.matrix[semester].push(old);
    return { wasSuccessful: true };
}

TrackMatrix.prototype.hasTakenCourse = function (courseId) {
    return this.hasTakenCourseInPast(MAX_SEMESTERS - 1, courseId);
}

TrackMatrix.prototype.hasTakenCourseInPast = function(semester, courseId) {
    for (var s = semester; s >= 0; s--) {
        if (this.indexOfCourse(s, courseId) != -1) {
            return s;
        }
    }
    return -1;
}

TrackMatrix.prototype.hasTakenCourseInFuture = function (semester, courseId) {
    for (var s = semester; s < MAX_SEMESTERS; s++) {
        if (this.indexOfCourse(s, courseId) != -1) {
            return s;
        }
    }
    return -1;
}

TrackMatrix.prototype.checkPreq = function (semester, node) {
    var dirty = new Array();
    var stack = new Array();
    var numInitialParents = node.parents.length;
    // start by pushing our initial nodes parents to the stack
    for (var i = 0; i < numInitialParents; i++) {
        stack.push({ node: this.nodes[node.parents[i]], index: semester });
    }

    while (stack.length > 0) {// while we have no more nodes to check
        var currFrame = stack.pop();// get the current stack frame we are looking at

        // first check if we have taken this class from the semester we are stupposed to
        // start checking it from
        var takenAt = this.hasTakenCourseInPast(currFrame.index, currFrame.node.course.id);
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

TrackMatrix.courseIsOfferedIn = function (semester, course) {
    var isSpring = semester & 1;// is odd and we assume we start in fall
    return (isSpring && course.isOfferedInSpring) || (!isSpring && course.isOfferedInFall);
}

TrackMatrix.prototype.removeCourse = function (semester, courseId) {
    // find out where it is in our current semester
    var index = this.indexOfCourse(semester, courseId);
    if (index == -1) {
        console.log("course not in matrix");
        return { wasSuccessful: false, errorType: "NOT_FOUND_IN_SEMESTER" };
    }

    // check its immediate children for conflicts
    var node = this.nodes[courseId];
    var conflicts = new Array();
    var numChildren = node.children.length;
    for (var i = 0; i < numChildren; i++) {
        var child = this.nodes[node.children[i]];
        // if we have one of our children taken in the future don't let us remove
        // this from the matrix
        var takenAt = this.hasTakenCourseInFuture(semester, child.course.id);
        if (takenAt != -1) {
            conflicts.push({ node: child, index: takenAt });
        }
    }

    // if we found any conflicts then don't let us remove
    if (conflicts.length > 0) {
        console.log("conflicts with classes in the future: %o", conflicts);
        return { wasSuccessful: false, errorType: "CONFLICTS", conflicts: conflicts };
    }

    var old = this.matrix[semester][index];
    this.coursePool.push(old);
    this.matrix[semester].splice(index, 1);
    return { wasSuccessful: true };
}

TrackMatrix.prototype.indexOfCourse = function (semester, courseId) {
    var numCourses = this.matrix[semester].length;
    for (var i = 0; i < numCourses; i++) {
        if (this.matrix[semester][i].id == courseId) {
            return i;
        }
    }
    return -1;
}

TrackMatrix.prototype.indexOfCourseInPool = function (courseId) {
    var numCourses = this.coursePool.length;
    for (var i = 0; i < numCourses; i++) {
        if (this.coursePool[i].id == courseId) {
            return i;
        }
    }
    return -1;
}

TrackMatrix.prototype.renderSemesters = function () {
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.renderSemester(i);
    }
}

TrackMatrix.prototype.renderSemester = function (semester) {
    var list = document.getElementById("course-list-" + semester);
    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }
    // this is equivelent to htmlTemplate
    //"<li id='semester-#{semesterId}-#{courseId}' class='course-item'>
    //    <div class='course-container'>
    //        <div class='course-name'>#{name}</div>
    //        <div class='course-description'>#{description}</div>
    //    </div>
    //</li>"
    var htmlTemplate =  "<li id='semester-{0}-{1}' class='course-item'><div class='course-container'><div class='course-name'>{2} ({1})</div><div class='course-description'>{3}</div></div></li>";
    var numCourses = this.matrix[semester].length;
    for (var i = 0; i < numCourses; i++) {
        var course = this.matrix[semester][i];
        var element = document.createElement('div');
        element.innerHTML = String.format(htmlTemplate, semester, course.id, course.name, course.description);
        while (element.children.length > 0) {
            list.appendChild(element.children[0]);
        }
    }
    //$('.course-container').hover(function () {
    //    $(this).animate({
    //        width: 350,
    //        height: 250,
    //        top: -80,
    //        left: -45
    //    }, 'fast');
    //    $(this).animate().css('box-shadow', '0 0 5px #000');
    //    $(this).css({
    //        zIndex: 100
    //    });
    //}, function () {
    //    $(this).animate().css('box-shadow', 'none')
    //    $(this).animate({
    //        width: 120,
    //        height: 80,
    //        top: 0,
    //        left: 0
    //    }, 'fast');
    //    $(this).css({
    //        zIndex: 1
    //    });
    //});
}

TrackMatrix.prototype.buildErrors = function(result) {
    this.errors = [];
    if (!result.wasSuccessful) {
        switch (result.errorType) {
            // add errors
            case "FULL": this.errors.push("Semeseter is full"); break;
            case "TAKEN": this.errors.push("Class already taken in semester " + result.takenIn); break;
            case "LOAD": this.errors.push("Failed to load course data"); break;
            case "OFFERING": this.errors.push("This course is not offered in that semester"); break;
            case "PREQ":
                var numDirty = result.dirty.length;
                for (var i = 0; i < numDirty; i++) {
                    this.errors.push("You must take " + result.dirty[i].node.course.name + " before semester " + (result.dirty[i].index + 1));
                }
                break;
                // remove errors
            case "NOT_FOUND_IN_SEMESTER": this.errors.push("Course not found in that semester"); break;
            case "NOT_FOUND_IN_HASH": this.errors.push("Course not found"); break;
            case "CONFLICTS":
                var numConflicts = result.conflicts.length;
                for (var i = 0; i < numConflicts; i++) {
                    this.errors.push("Removing this course would invalidate your future course " + result.conflicts[i].node.course.name);
                }
                break;
        }
    }
    this.renderErrors();
}

TrackMatrix.prototype.renderErrors = function () {
    var list = document.getElementById("error-list");
    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }
    var numErrors = this.errors.length;
    for (var i = 0; i < numErrors; i++) {
        var error = this.errors[i];
        var entry = document.createElement("li");
        entry.id = "error-" + i;
        entry.appendChild(document.createTextNode(error));
        list.appendChild(entry);
    }
}

TrackMatrix.prototype.renderCourses = function () {
    var list = document.getElementById("course-pool");
    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }
    var numCourses = this.coursePool.length;
    for (var i = 0; i < numCourses; i++) {
        var courseId = this.coursePool[i].id;
        var entry = document.createElement("li");
        entry.id = "course-" + courseId;
        var self = this;
        entry.onclick = function () {
            var parts = this.id.split('-');
            var cid = parts[1];
            var result = self.addCourse(self.selectedSemester, cid);
            self.buildErrors(result);
            if (result.wasSuccessful) {
                self.renderSemester(self.selectedSemester);
                self.renderCourses();
            }
        }
        entry.appendChild(document.createTextNode(courseId));
        list.appendChild(entry);
    }
}


