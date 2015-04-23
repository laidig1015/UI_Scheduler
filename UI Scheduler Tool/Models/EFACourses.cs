using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI_Scheduler_Tool.Models.Extensions;
using System.Data.Entity.Migrations;

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

        public EFACourses Get(DataContext db)
        {
            return db.EFACourses.UniqueWhere(this, e => e.EFAID == EFAID && e.CourseID == CourseID);
        }

        public EFACourses Add(DataContext db)
        {
            EFACourses efa = Get(db);
            db.EFACourses.AddOrUpdate(efa);
            return efa;
        }
    }
}