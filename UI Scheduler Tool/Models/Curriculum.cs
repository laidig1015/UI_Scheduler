using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI_Scheduler_Tool.Models.Extensions;
using System.Data.Entity.Migrations;

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

        public Curriculum Get(DataContext db)
        {
            return db.Curricula.UniqueWhere(this, c => c.CourseID == CourseID && c.TrackID == TrackID);
        }

        public Curriculum Add(DataContext db)
        {
            Curriculum curiculum = Get(db);
            db.Curricula.AddOrUpdate(curiculum);
            return curiculum;
        }
    }
}