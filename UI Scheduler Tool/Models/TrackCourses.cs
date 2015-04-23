using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI_Scheduler_Tool.Models.Extensions;
using System.Data.Entity.Migrations;

namespace UI_Scheduler_Tool.Models
{
    public class TrackCourses
    {
        public int ID { get; set; }
        public int TrackID { get; set; }
        public int CourseID { get; set; }
        public int EFAType { get; set; }

        public virtual Track Track { get; set; }
        public virtual Course Course { get; set; }

        public TrackCourses Get(DataContext db)
        {
            return db.TrackCourses.UniqueWhere(this, t =>
                                            t.TrackID == TrackID
                                            && t.CourseID == CourseID
                                            && t.EFAType == EFAType);
        }

        public TrackCourses Add(DataContext db)
        {
            TrackCourses track = Get(db);
            db.TrackCourses.AddOrUpdate(track);
            return track;
        }
    }
}