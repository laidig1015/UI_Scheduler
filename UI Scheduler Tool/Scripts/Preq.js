function Course(name, id, hours, description, hasFall, hasSpring, parents, children) {
    this.name = name;
    this.id = id;
    this.hours = hours;
    this.hasFall = (!hasFall) ? false : hasFall;
    this.hasSprint = (!hasSpring) ? false : hasSpring;
    this.description = (!description) ? '' : description;
    this.parents = (!parents) ? [] : parents;
    this.children = (!children) ? [] : children;
}

function PreqNode(course) {
    this.course = course;
    this.parents = [];
    this.children = [];
}

function PreqTable(courses) {
    this.table = {};
    for (course in courses) {
        table[course.id] = new PreqNode(course);
    }
    for (course in courses) {
        var node = table[course.id];
        for (edge in coures.parents) {
            node.parents.push(table[edge.parent.id]);
        }
        for (edge in course.children) {
            node.children.push(table[edge.child.id]);
        }
    }
}

var MAX_SEMSTERS = 8;
var MAX_CLASSES = 20;

function TrackMatrix(preqTable) {
    this.matrix = new Array(MAX_SEMSTERS);
    for (var i = 0; i < MAX_SEMSTERS; i++) {
        this.matrix[i] = [];
    }
    this.preqTable = preqTable;
    this.totalSemsterHours = 0;
}

TrackMatrix.prototype.add = function (semster, courseID) {
    
}

TrackMatrix.prototype.remove = function (semster, courseID) {

}