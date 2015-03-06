using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Maui
{
    public class MauiCourse
    {
        public string title { get; set; }
        public string catalogDescription { get; set; }
        public string lastTaught { get; set; }
        public int lastTaughtId { get; set; }
        public string lastTaughtCode { get; set; }
        public string courseNumber { get; set; }
        public string legacyCourseNumber { get; set; }
        public string creditHours { get; set; }// should be an int most of the time
    }
}