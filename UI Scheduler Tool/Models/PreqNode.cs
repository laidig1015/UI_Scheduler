using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models.Json
{
    // NOTE: this is supposed to be serialied to json so we us the pascalCase convension
    // that is more common in js 
    public class PreqNode
    {
        public string name { get; set; }
        public string id { get; set; }
        public string hours { get; set; }
        public bool isOfferedInFall { get; set; }
        public bool isOfferedInSpring { get; set; }
        public string description { get; set; }
        public string[] parents { get; set; }
        public string[] children { get; set; }

        public PreqNode(Course c)
        {
            name = c.CourseName;
            id = c.CourseNumber;
            hours = c.CreditHours;
            isOfferedInFall = true;// TODO
            isOfferedInSpring = true;
            parents = c.Parents.Select(p => p.Parent.CourseNumber).ToArray();
            children = c.Children.Select(p => p.Child.CourseNumber).ToArray();
        }
    }
}