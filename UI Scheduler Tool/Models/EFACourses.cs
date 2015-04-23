using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class EFACourses
    {
        public int ID { get; set; }
        public int EFAID { get; set; }
        public int CourseID { get; set; }
        public int EFAType { get; set; }

        public virtual EFA EFA { get; set; }
        public virtual Course Course { get; set; }
    }
}