using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models.Json
{
    // NOTE: this is supposed to be serialied to json so we us the pascalCase convension
    // that is more common in js 
    public class TrackMatrixNode
    {
        public string name { get; set; }
        public string id { get; set; }
        public string hours { get; set; }
        public string description { get; set; }

        public TrackMatrixNode(Course c)
        {
            name = c.CourseName;
            id = c.CourseNumber;
            hours = c.CreditHours;
            description = c.CatalogDescription;
        }
    }
}