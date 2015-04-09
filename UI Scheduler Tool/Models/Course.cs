using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public partial class Course
    {
        public int ID { get; set; }
        public string CourseName { get; set; }
        public string CatalogDescription { get; set; }
        public string CourseNumber { get; set; }
        public string CreditHours { get; set; }
        public int Occurence { get; set; }
        public int LastTaughtID { get; set; }

        public virtual ICollection<PreqEdge> Parents { get; set; }
        public virtual ICollection<PreqEdge> Children { get; set; }
        public virtual ICollection<CourseSection> CourseSections { get; set; }

        public Course()
        {
            Parents = new HashSet<PreqEdge>();
            Children = new HashSet<PreqEdge>();
            CourseSections = new HashSet<CourseSection>();
        }

        public string CourseSubject
        {
            get
            {
                return CourseNumber.Substring(0, CourseNumber.IndexOf(':'));
            }
        }

        public int CourseIdentity
        {
            get
            {
                int i = CourseNumber.IndexOf(':');
                return Int32.Parse(CourseNumber.Substring(i + 1, CourseNumber.Length - i));
            }
        }
    }
}