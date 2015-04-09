using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class PreqNode
    {
        public Course CourseInfo { get; set;}
        public List<PreqNode> Children { get; set; }
        public List<PreqNode> Parents { get; set; }

        public PreqNode(Course c)
        {
            CourseInfo = c;
            Children = new List<PreqNode>();
            Parents = new List<PreqNode>();
        }
    }
}