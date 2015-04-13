var Preq = {};

var MAX_SEMESTERS = 8;
var MAX_CLASSES = 20;

function TrackMatrix() {
    this.matrix = new Array(MAX_SEMESTERS);
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.matrix[i] = new Array(MAX_CLASSES);
    }
    this.nodes = {};
    this.totalSemsterHours = 0;
}

TrackMatrix.prototype.seedCurriculum = function (track) {
    var self = this;
    $.get("/Track/GetCurriculum", "trackName=" + track, function (curriculum, status) {
        console.log("got curriculum: %o", curriculum);
        self.matrix = curriculum;
        self.render;
        return true;
    }, "json").fail(function (err, status) {
        console.log("error getting nodes: %s (%s)", err, status);
    });
    return false;
}

TrackMatrix.prototype.loadCourses = function (query) {
    // load our edges from our controller and call our build
    // function to add it to our table
    var self = this;
    $.get("/Track/GetNodes", "query=" + query, function (nodes, status) {
        console.log("got nodes: %o", nodes);
        self.addNodes(nodes);
        return true;
    }, "json").fail(function (err, status) {
        console.log("error getting nodes: %s (%s)", err, status);
    });
    return false;
}

TrackMatrix.prototype.addNodes = function (nodes) {
    var nodesLen = nodes.length;
    for (var i = 0; i < nodesLen; i++) {
        var node = nodes[i];
        if (!(node.id in this.nodes)) {
            this.nodes[node.id] = node;
        }
    }
    console.log("updated preq: %o", this.nodes);
}

TrackMatrix.prototype.addCourse = function (semester, nodeId) {
    // make sure we have more room in our semester
    if (this.matrix[semester].length >= MAX_CLASSES) {
        console.log("max classes reached");
        return { wasSuccessful: false, errorType: "FULL" };
    }

    // first make sure we haven't taken this class already
    var takenIn = this.hasTakenCourse(nodeId);
    if (takenIn != -1) {
        console.log("class already taken in semster %d", takenIn + 1);// offset from 0 index
        return { wasSuccessful: false, errorType: "TAKEN", takenIn: takenIn };
    }

    // first find our course node in our hash
    // if we don't have it try to update from our database
    if (!this.resolveNode(nodeId)) {
        console.log("failed to load %s from db", nodeId);
        return { wasSuccessful: false, errorType: "LOAD" };
    }

    // find the current node
    var node = this.nodes[nodeId];

    // first check if the course is offered in this semester
    if (TrackMatrix.courseIsOfferedIn(semester, node)) {
        console.log("%s is not available in %s", nodeId, (semester & 1 ? "spring" : "fall"));
        return { wasSuccessful: false, errorType: "OFFERING" };
    }

    // check if we have the required prequisties to take the course
    var dirty = this.checkPreq(semester, node);
    if (dirty.length > 0) {
        console.log("preq error when adding course, dirty %o", dirty);
        return { wasSuccessful: false, errorType: "PREQ", dirty: dirty };
    }

    // finally add it to our matrix
    this.matrix[semester].push(node.id);
    this.renderSemester(semester);
    return { wasSuccessful: true };
}

TrackMatrix.prototype.resolveNode = function (nodeId) {
    // check if we don't have it in our hash table
    // if we don't then try to load it from our db
    return (!(nodeId in this.nodes) && !this.loadNodes(nodeId));
}

TrackMatrix.prototype.hasTakenCourse = function (courseId) {
    return hasTakenCourseInPast(MAX_SEMESTERS - 1, courseId);
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
        var takenAt = this.hasTakenCourseInPast(currFrame.index, currFrame.node.id);
        if (takenAt == -1) {
            // if we didnt' take it then mark it as dirty
            dirty.push(currFrame);
        } else {
            // if we did take it then add its parents to the stack to be checked
            // since we did find it all of the parents should start their search
            // from the index we found the class at
            var numParents = curr.node.parents.length;
            for (var i = 0; i < numParents; i++) {
                stack.push({ node: curr.node, index: takenAt });
            }
        }
    }
    return dirty;// return an array of our dirty nodes
}

TrackMatrix.courseIsOfferedIn = function (semsterId, course) {
    var isSpring = semsterId & 1;// is odd and we assume we start in fall
    return ((isSpring && course.isOfferedInSpring) || (!isSpring && course.isOfferedInFall));
}

TrackMatrix.prototype.removeCourse = function (semster, courseId) {
    // find out where it is in our current semester
    var index = this.indexOfCourse(semester, courseId);
    if (index == -1) {
        console.log("course not in matrix");
        return { wasSuccessful: false, errorType: "NOT_FOUND_IN_SEMESTER" };
    }

    // make sure we have the node information
    if (!(courseId in this.nodes)) {
        console.log("coures not found in hash! %s", courseId);
        return { wasSuccessful: false, errorType: "NOT_FOUND_IN_HASH" }; 
    }

    // check its immediate children for conflicts
    var node = this.nodes[courseId];
    var conflicts = new Array();
    var numChildren = node.children.length;
    for (var i = 0; i < numChildren; i++) {
        var child = this.nodes[node.children[i].id];
        // if we have one of our children taken in the future don't let us remove
        // this from the matrix
        var takenAt = this.hasTakenCourseInFuture(semster, child.id);
        if (takenAt != -1) {
            conflicts.push({ node: child, index: takenAt });
        }
    }

    // if we found any conflicts then don't let us remove
    if (conflicts.length > 0) {
        console.log("conflicts with classes in the future: %o", conflicts);
        return { wasSuccessful: false, errorType: "CONFLICTS", conflicts: conflicts };
    }

    this.matrix[semster].splice(index, 1);
    this.renderSemester(semester);
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

TrackMatrix.prototype.render = function () {
    for (var i = 0; i < MAX_SEMESTERS; i++) {
        this.renderSemester(i);
    }
}

TrackMatrix.prototype.renderSemester = function (semester) {
    var list = document.getElementById("semester-" + semester);
    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }
    var numCourses = this.matrix[semester].length;
    for (var i = 0; i < numCourses; i++) {
        var course = this.matrix[semester][i];
        var entry = document.createElement("li");
        entry.id = "course-" + course.id + "-" + semester;
        var self = this;
        entry.onclick = function () {
            var parts = this.id.split('-');
            var s = parseInt(parts[2]);
            var courseId = parts[1];
            var index = self.indexOfCourse(s, courseId);
            self.matrix[s].splice(index, 1);
            self.renderSemester(s);
        }
        entry.appendChild(document.createTextNode(this.matrix[semester][i].id));
        list.appendChild(entry);
    }
}


