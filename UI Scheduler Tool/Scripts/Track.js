// NAMESPACE //
var Track = {};

Track.__UID = 0;
Track.addNewElementTo = function (list, course, state, type) {
    // NOTE: we imply ok for state by default because it will be what most states will be
    // emulating this template
    //<li id='semester-#{courseId}' class='course-ok'>
    //    <h3 class='course-name'>#{name} (#{courseId})</h3>
    //    <a id='course-toggle-info-#{uniqueId}' class='info-toggle' href='#>Description</a>
    //    <p id='course-content-info-#{uniqueId}' class='course-info-content'>#{description}</p>
    //    <a id='course-toggle-error-#{uniqueId}' class='error-toggle' href='#'>Error</a>
    //    <p id='course-content-error-#{uniqueId}' class='course-error-content'></p>
    //</li>
    var template = "<li id='course-{0}' class='{4}'><h3 class='course-name'>{2} ({0})</h3><a id='course-toggle-info-{1}' class='info-toggle' href='#'>Description</a><p id='course-content-info-{1}' class='course-info-content'>{3}</p><a id='course-toggle-error-{1}' class='error-toggle' href='#'>Error</a><p id='course-content-error-{1}' class='course-error-content'></p></li>";
    var element = document.createElement('div');
    element.innerHTML = String.format(template,
                    course.id,
                    Track.__UID,
                    course.name,
                    course.description,
                    'course-' + state + ' course-' + type);
    list.append(element.innerHTML);
    var uid = Track.__UID++;
    // DON'T FORGET SCOPING!!! http://stackoverflow.com/questions/8909652/adding-click-event-listeners-in-loop
    infoToggleName = '#course-toggle-info-' + uid;
    infoContentName = '#course-content-info-' + uid;
    if (typeof window.addEventListener === 'function') {
        (function (_infoContentName) {
            $(infoToggleName).click(function () {
                $(_infoContentName).slideToggle("fast");
            });
        })(infoContentName);
    }

    errorToggleName = '#course-toggle-error-' + uid;
    errorContentName = '#course-content-error-' + uid;
    if (typeof window.addEventListener === 'function') {
        (function (_errorContentName) {
            $(errorToggleName).click(function () {
                $(_errorContentName).slideToggle("fast");
            });
        })(errorContentName);
    }
}

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

if (!String.generateRange) {
    String.generateRange = function (begin, end, prefix, delim) {
        if(typeof(delim) === 'undefined') delim = ',';
        var list = [];
        for (var i = begin; i < end; i++) {
            list.push(prefix + i);
        }
        return list.join(',');
    }
}

Track.loadNodes = function(route, args, context, callback) {
    $.get(route, args, function (nodes, status) {
        if (!nodes) {
            console.log("error getting nodes for %s", route);
            return false;
        }
        console.log('nodes for %s: %o', route, nodes);
        var regexFilter = /(<([^>]+)>)/ig;// remove any html tags we might find in the desc
        for(var i = 0; i < nodes.length; i++) {
            nodes[i].course.description = nodes[i].course.description.replace(regexFilter, '');
        }
        callback(context, nodes);
    }, "json").fail(function (err, status) {
        console.log("error getting nodes for %s: %s (%s)", route, err, status);
    });
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

TrackModel.prototype.find = function(courseId) {
    for(var i = 0; i < MAX_SEMESTERS; i++) {
        var item = this.matrix[i].find(courseId);
        if(item) {
            return item;
        }
    }
    return null;
}

TrackModel.prototype.loadTrack = function (trackId, callback) {
    Track.loadNodes('/Track/GetTrackNodes', "trackId=" + trackId, this, function (self, nodes) {
        for (var i = 0; i < MAX_SEMESTERS; i++) {
            self.matrix[i].clear();
            $('#semester-' + i).empty();
        }
        self.addNodes(nodes);
        callback();
    });
}

TrackModel.prototype.addNodes = function (nodes) {
    for (var i = 0; i < nodes.length; i++) {
        var node = nodes[i];
        // always trust the db source for now
        if (node.index >= 0) {
            this.matrix[node.index].add(new CourseItem(node.course));
        }
        if (!(node.id in this.nodes)) {
            this.nodes[node.course.id] = node;
            this.nodes[node.course.id].isOffered = true;
            this.nodes[node.course.id].preqDirty = [];
        }
    }
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

TrackModel.prototype.checkPreqs = function (semester, node) {
    var dirty = new Array();
    var stack = new Array();
    var numInitialParents = node.parents.length;
    // start by pushing our initial nodes parents to the stack
    for (var i = 0; i < numInitialParents; i++) {
        stack.push({ node: this.nodes[node.parents[i]], index: semester });
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

TrackModel.prototype.add = function (semester, courseItem) {
    var actions = [];
    var course = courseItem.course;

    // find the current node
    var node = this.nodes[course.id];

    // make sure we haven't taken this course in the past
    var takenIn = this.hasCourse(course.id);
    if (takenIn != -1) {
        console.log("class already taken in: %d", takenIn);
        actions.push({ type: 'ALREADY_TAKEN', takenIn: takenIn });
    }

    // update if this new item is validated for this semester
    courseItem.isOffered = TrackModel.isOfferedIn(semester, node.course);
    if (!courseItem.isOffered) {
        console.log("class not offered!");
        actions.push({ type: 'NOT_OFFERED' });
    }

    // check if we have the required prequisties to take the course
    var newDirty = this.checkPreqs(semester, node);
    if(newDirty.length > 0) {
        console.log('preq errors: %o', newDirty);
    }
    actions.push({ type: 'PREQ', dirty: newDirty });// always add errors
    courseItem.preqDirty = newDirty;

    this.matrix[semester].add(courseItem);
    return actions;
}

TrackModel.prototype.remove = function (semester, courseId) {
    var result = { wasRemoved: false, errors: [], item: null };

    // make sure we have the course in this semester
    if (!this.matrix[semester].hasCourse(courseId)) {
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
        var takenAt = this.willTake(semester, child.course.id);
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

    this.matrix[semester].remove(courseId);
    result.item = this.nodes[courseId];
    //result.item = this.matrix[semester].remove(courseId);
    result.wasRemoved = true;
    return result;
}

// COURSE LIST //
function CourseList() {
    this.clear();
}

CourseList.prototype.clear = function () {
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

// COURSE ITEM //
function CourseItem(course) {
    this.course = course;
    this.preqDirty = [];
    this.isOffered = true;
}

// TRACK VIEW //
function TrackView(model) {
    this.model = model;
    this._lastRemoved = null;
    this._enableSorting();
}

TrackView.prototype._enableSorting = function() {
    // add links to itself
    var self = this;
    var allSemesters = String.generateRange(0, MAX_SEMESTERS, '#semester-');
    $('.course-list').sortable({
        connectWith: '.course-list',
        remove: function (event, ui) {
            console.log('remove: target: %s item: %s', event.target.id, ui.item.context.id);
            var targetParts = event.target.id.split('-');
            if (targetParts[0] == 'semester') {
                var semester = parseInt(targetParts[1], 10);
                var courseId = ui.item.context.id.split('-')[1];
                var result = self.model.remove(semester, courseId);
                self._lastRemoved = result.item;
            } else {
                self._lastRemove = null;
            }
        },
        receive: function (event, ui) {
            console.log('receive: target: %s item: %s', event.target.id, ui.item.context.id);
            var targetParts = event.target.id.split('-');
            if (targetParts[0] == 'semester') {
                var semester = parseInt(targetParts[1], 10);
                var courseId = ui.item.context.id.split('-')[1];

                var actions = self.model.add(semester, self.model.nodes[courseId])//self._lastRemoved);
                var errors = [];
                for (var i = 0; i < actions.length; i++) {
                    var action = actions[i];
                    switch (action.type) {
                        case 'ALREADY_TAKEN':
                            errors.push('this class was already taken in semester ' + (takenIn + 1));
                            break;// FAIL HARD?
                        case 'NOT_OFFERED':
                            errors.push('this class is not offered in this semester');
                            break;
                        case 'PREQ':
                            if (action.dirty.length > 0) {
                                var dirtyErrors = [];
                                for (var j = 0; j < action.dirty.length; j++) {
                                    dirtyErrors.push(action.dirty[i].node.course.name);
                                }
                                errors.push('you must take these classes first: ' + dirtyErrors.join(', '));
                            }
                            break;
                        default:
                            console.log('unknown error: %s', action.type);
                            continue;
                    }
                }
                self.setCourseState(courseId, errors.length > 0 ? 'error' : 'ok');
                var element = $(TrackView.escapeQuery('#course-' + courseId + ' > .course-error-content'));
                if (errors.length > 0) {
                    element.html(errors.join('\n\n'));
                } else {
                    element.html('');
                    if (!element.is(':hidden')) {
                        element.slideToggle('fast');
                    }
                }
            }
        }
    }).disableSelection();
}

TrackView.prototype.clear = function (semesterIndex) {
    $('#semester-' + semesterIndex).empty();
}

TrackView.prototype.clearAll = function () {
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.clear(i);
    }
}

TrackView.getElement = function (courseId) {
    return $('#course-' + courseId.replace(':', '\\:'));// escape colon in jquery
}

TrackView.escapeQuery = function (selector) {
    return selector.replace(':', '\\:');
}

TrackView.prototype.setCourseState = function (courseId, state) {
    switch (state) {
    case 'ok': case 'warning': case 'error': break;
    default: return;
    };
    var element = TrackView.getElement(courseId);
    if (!element) return;
    element.attr('class', '');
    element.addClass('course-' + state);
}

TrackView.prototype.render = function () {
    var infoToggle, infoContent, errorToggle, errorContent;

    for (var s = 0; s < MAX_SEMESTERS; s++) {
        this.clear(s);
        var semester = this.model.matrix[s];
        for (var courseId in semester.courses) {
            var item = semester.courses[courseId];
            Track.addNewElementTo($('#semester-' + s), item.course, 'ok', 'other');
        }
    }
}

// EFA //
function EFAModel(trackCallback, efaCallback, model) {
    this.tracks = null;
    this.efas = null;
    this.model = model;
    this.lastTrackId = -1;
    this.lastEFAId = -1;
    this.onTrack = trackCallback;
    this.onEFA = efaCallback;
    var load = $('#efa-load');
    load.prop('disabled', true);

    var self = this;
    load.click(function () {
        var trackId = parseInt($('#track-select').val());
        if (trackId != self.lastTrackId) {
            console.log('loading new track: %d', trackId);
            self.onTrack(trackId);
        }
        self.lastTrackId = trackId;

        var efaId = parseInt($('#efa-select').val());
        if (efaId != self.lastEFAId) {
            console.log('loading new efas: %d', efaId);
            self.onEFA(efaId);
        }
        self.lastEFAId = efaId;
    });
    this.loadOptions();
}

EFAModel.prototype.loadOptions = function () {
    var self = this;
    $.get("/Track/GetEFASeed", '', function (seed, status) {
        if (!seed) {
            console.log("error getting seed!");
            return false;
        }
        console.log("got seed: %o", seed);
        self.tracks = seed.tracks;
        self.efas = seed.efas;
        $("#track-select").change(function () {
            self.renderEFAOptions(this.selectedIndex);
        });
        self.renderTrackOptions();
        self.renderEFAOptions(0);
        $('#efa-load').prop('disabled', false);
    }, "json").fail(function (err, status) {
        console.log("error getting nodes: %s (%s)", err, status);
    });
}

EFAModel.prototype.loadEFA = function (efaId, callback) {
    Track.loadNodes('/Track/GetEFANodes', "efaId=" + efaId, this, function (self, nodes) {
        var names = ['bredth', 'depth', 'upper', 'technical'];
        for (var i = 0; i < names.length; i++) {
            var pool = $('#' + names[i] + '-list');
            pool.empty();
            var items = nodes[names[i]];
            self.model.addNodes(items);
            for (var j = 0; j < items.length; j++) {
                var node = items[j];
                Track.addNewElementTo(pool, node.course, 'ok', names[node.type]);
            }
        }
    });
}

EFAModel.prototype.renderTrackOptions = function () {
    var options = $("#track-select");
    for (var i = 0; i < this.tracks.length; i++) {
        var track = this.tracks[i];
        options.append($("<option />").val(track.id).text(track.name));
    }
}

EFAModel.prototype.renderEFAOptions = function (index) {
    var options = $('#efa-select');
    options.empty();
    for (var i = 0; i < this.efas[index].length; i++) {
        var efa = this.efas[index][i];
        options.append($("<option />").val(efa.id).text(efa.name));
    }
}