using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models.Json
{
    public class JEFAData
    {
        public List<JNode> bredth { get; set; }
        public List<JNode> depth { get; set; }
        public List<JNode> upper { get; set; }
        public List<JNode> technical { get; set; }

        public JEFAData(List<EFACourses> courses)
        {
            bredth = new List<JNode>();
            depth = new List<JNode>();
            upper = new List<JNode>();
            technical = new List<JNode>();
            foreach(var course in courses)
            {
                JNode node = new JNode(course.Course) { type = course.EFAType };
                switch(course.EFAType)
                {
                case (int)EFAType.BREDTH: bredth.Add(node); break;
                case (int)EFAType.DEPTH: depth.Add(node); break;
                case (int)EFAType.UPPER: upper.Add(node); break;
                case (int)EFAType.TECHNICAL: technical.Add(node); break;
                }
            }
        }
    }
}