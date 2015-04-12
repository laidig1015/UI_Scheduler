var Preq = {};

var MAX_SEMSTERS = 8;
var MAX_CLASSES = 20;

function TrackMatrix() {
    this.matrix = new Array(MAX_SEMSTERS);
    for (var i = 0; i < MAX_SEMSTERS; i++) {
        this.matrix[i] = new Array(MAX_CLASSES);
    }
    this.preq = {};
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
        if (!(node.id in this.preq)) {
            this.preq[node.id] = node;
        }
    }
    console.log("updated preq: %o", this.preq);
}

TrackMatrix.prototype.addCourse = function (semsterId, nodeId) {
    // first find our course node in our hash
    // if we don't have it try to update from our database
    if (!(nodeId in this.preq) && !this.loadNodes(nodeId)) {
        console.log("failed to load %s from db", nodeId);
        return false;
    }
    var node = this.preq[nodeId];

    // first check if the course is offered in this semester
    if (TrackMatrix.nodeIsOfferedIn(semsterId, node)) {
        console.log("%s is not available in %s", nodeId, (semsterId & 1 ? "spring" : "fall"));
        return false;
    }

    // go through all parents of our nodes 
    var curr = node;
    var childI = 0;
    var semester = semsterId;
    while (childI > 0) {
        if (this.nodeIsInSemster(semester, curr)) {

        }
    }
}

TrackMatrix.nodeIsOfferedIn = function (semsterId, node) {
    var isSpring = semsterId & 1;// is odd and we assume we start in fall
    return ((isSpring && node.isOfferedInSpring) || (!isSpring && node.isOfferedInFall));
};

TrackMatrix.prototype.nodeIsInSemster = function (semester, node) {
    return node.id in this.matrix[semester];
};

TrackMatrix.prototype.removeCourse = function (semster, courseID) {

};


