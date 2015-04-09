using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace UI_Scheduler_Tool.Models
{
    public partial class UIContext : DbContext
    {
        public UIContext()
            : base("DefaultConnection")
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseSection> CourseSections { get; set; }
        public virtual DbSet<PreqEdge> PreqEdges { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
    }
}