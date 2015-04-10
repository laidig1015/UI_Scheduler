using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class TrackMatrix
    {
        public const int MAX_SEMSESTERS = 8;

        private List<Course>[] _matrix;
        private PreqTable _preq;

        public int TotalCreditHours { get; private set; }

        public TrackMatrix(PreqTable table)
        {
            _matrix = new List<Course>[MAX_SEMSESTERS];
            for (int i = 0; i < MAX_SEMSESTERS; i++)
            {
                _matrix[i] = new List<Course>();
            }
            _preq = table;
        }

        public void AddCourses(int semester, List<Course> courses)
        {
            for (int i = 0; i < courses.Count; i++)
                AddCourse(semester, courses[i]);
        }

        public void AddCourse(int semester, Course course)
        {
            _matrix[semester].Add(course);
        }
    }
}