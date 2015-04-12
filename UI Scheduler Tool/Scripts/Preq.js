var Preq = {};

var MAX_SEMSTERS = 8;
var MAX_CLASSES = 20;

function TrackMatrix() {
    this.matrix = new Array(MAX_SEMSTERS);
    for (var i = 0; i < MAX_SEMSTERS; i++) {
        this.matrix[i] = new Array(MAX_CLASSES);
    }
    this.nodes = {};
    this.totalSemsterHours = 0;
}

TrackMatrix.prototype.loadNodes = function (query) {
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
    // first make sure we haven't taken this class already
    var takenIn = this.hasTaken(nodeId);
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

    var node = this.nodes[nodeId];

    // first check if the course is offered in this semester
    if (TrackMatrix.nodeIsOfferedIn(semester, node)) {
        console.log("%s is not available in %s", nodeId, (semester & 1 ? "spring" : "fall"));
        return { wasSuccessful: false, errorType: "OFFERING" };
    }

    // check if we have the required prequisties to take the course
    var dirty = this.checkPreq(semester, node);
    if (dirty.length > 0) {
        return { wasSuccessful: false, errorType: "PREQ", dirty: dirty };
    }

}

TrackMatrix.prototype.resolveNode = function (nodeId) {
    // check if we don't have it in our hash table
    // if we don't then try to load it from our db
    return (!(nodeId in this.nodes) && !this.loadNodes(nodeId));
}

TrackMatrix.prototype.hasTaken = function (node) {
    return hasTakenSince(MAX_SEMSTERS - 1, node);
}

TrackMatrix.prototype.hasTakenSince = function(semester, nodeId) {
    for (var s = semester; s >= 0; s--) {
        if (this.matrix[s].indexOf(nodeId) != -1) {
            return s;
        }
    }
    return -1;
}

TrackMatrix.prototype.checkPreq = function (semester, node) {
    // go through all parents of our nodes
    var dirty = new Array();
    var stack = new Array();
    var numInitialParents = node.parents.length;
    // start by pushing our initial nodes parents to the stack
    for (var i = 0; i < numInitialParents; i++) {
        stack.push({ node: this.nodes[node.parents[i]], index: semsterId });
    }

    while (stack.length > 0) {// while we have no more nodes to check
        var currFrame = stack.pop();// get the current stack frame we are looking at

        // first check if we have taken this class from the semester we are stupposed to
        // start checking it from
        var takenAt = this.hasTakenSince(currFrame.index, currFrame.node.id);
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

TrackMatrix.nodeIsOfferedIn = function (semsterId, node) {
    var isSpring = semsterId & 1;// is odd and we assume we start in fall
    return ((isSpring && node.isOfferedInSpring) || (!isSpring && node.isOfferedInFall));
}

TrackMatrix.prototype.removeCourse = function (semster, courseID) {

}


