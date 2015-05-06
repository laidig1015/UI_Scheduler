using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models.Json
{
    public class JCourse
    {
        public string name { get; set; }
        public string id { get; set; }
        public string hours { get; set; }
        public bool isOfferedInFall { get; set; }
        public bool isOfferedInSpring { get; set; }
        public string description { get; set; }

        public JCourse(Course c)
        {
            name = c.CourseName;
            id = c.CourseNumber;
            hours = c.CreditHours;
            isOfferedInFall = c.IsOfferedInFall;
            isOfferedInSpring = c.IsOfferedInSpring;
            description = c.CatalogDescription;
        }
    }

    // NOTE: this is supposed to be serialied to json so we us the pascalCase convension
    // that is more common in js 
    public class JNode
    {
        public JCourse course { get; set; }
        public int type { get; set; }
        public int index { get; set; }
        public string[] parents { get; set; }
        public string[] children { get; set; }

        public JNode(Course c)
        {
            course = new JCourse(c);
            type = (int)EFAType.OTHER;
            index = -1;
            parents = c.Parents.Select(p => p.Parent.CourseNumber).ToArray();
            children = c.Children.Select(p => p.Child.CourseNumber).ToArray();
        }
    }
}