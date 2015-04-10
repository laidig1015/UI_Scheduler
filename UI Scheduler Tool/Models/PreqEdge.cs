using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class PreqEdge
    {
        public int Id { get; set; }
        public int ParentID { get; set; }
        public int ChildID { get; set; }
        public bool IsRequired { get; set; }

        public virtual Course Parent { get; set; }
        public virtual Course Child { get; set; }
    }
}