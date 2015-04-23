using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI_Scheduler_Tool.Models.Extensions;
using System.Data.Entity.Migrations;

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

        public PreqEdge Get(DataContext db)
        {
            return db.PreqEdges.UniqueWhere(this, e => e.ParentID == ParentID && e.ChildID == ChildID);
        }

        public PreqEdge Add(DataContext db)
        {
            PreqEdge edge = Get(db);
            db.PreqEdges.AddOrUpdate(edge);
            return edge;
        }
    }
}