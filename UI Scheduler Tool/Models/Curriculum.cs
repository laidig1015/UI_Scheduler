using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class Curriculum
    {
        public int ID { get; set; }
        public int TrackID { get; set; }
        public int CourseID { get; set; }
        public int SemesterIndex { get; set; }

        public virtual Track Track { get; set; }
        public virtual Course Course { get; set; }
    }
}