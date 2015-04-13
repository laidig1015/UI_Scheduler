using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models.Json
{
    // NOTE: this is supposed to be serialied to json so we us the pascalCase convension
    // that is more common in js 
    public class JPreqNode
    {
        public JCourse course { get; set; }
        public int index { get; set; }
        public string[] parents { get; set; }
        public string[] children { get; set; }

        public JPreqNode(Course c)
        {
            course = new JCourse(c);
            index = -1;
            parents = c.Parents.Select(p => p.Parent.CourseNumber).ToArray();
            children = c.Children.Select(p => p.Child.CourseNumber).ToArray();
        }
    }
}