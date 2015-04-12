var EFA = {};

var REQUIRED_NUM_BREDTH = 1;
var REQUIRED_NUM_DEPTH = 1;
var REQUIRED_NUM_UPPER_LEVEL = 2;
var REQUIRED_NUM_TECHNICAL = 3;
var REQUIRED_ADDITIONAL = 2;

function RequiredCoursePool(minNeeded, allowed) {
    this.allowed = allowed;
    this.selected = {};
    this.numSelected = 0;
    this.minNeeded = minNeeded;
}

RequiredCoursePool.prototype.add = function (courseId) {
    if (!(courseId in this.selected) && courseId in this.allowed) {
        // just set it to our key for now since we are only using it to check for repeats
        this.selected[courseId] = courseId;
        this.numSelected++;
    }
}

RequiredCoursePool.prototype.remove = function (courseId) {
    if (courseId in this.selected) {
        delete this.selected[courseId];
        this.numSelected--;
    }
}

RequiredCoursePool.prototype.isComplete = function () {
    return this.numSelected >= this.minNeeded;
}

RequiredCoursePool.prototype.numNeeded = function () {
    return (this.isComplete()) ? 0 : this.minNeeded - this.numSelected;
}