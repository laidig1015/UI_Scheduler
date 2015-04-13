using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace UI_Scheduler_Tool.Models
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<PreqEdge> PreqEdges { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PreqEdge>()
                        .HasRequired(e => e.Parent)
                        .WithMany(c => c.Children)
                        .HasForeignKey(e => e.ParentID)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<PreqEdge>()
                        .HasRequired(e => e.Child)
                        .WithMany(c => c.Parents)
                        .HasForeignKey(e => e.ChildID)
                        .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}