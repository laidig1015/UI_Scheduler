namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UIContext : DbContext
    {
        public UIContext()
            : base("name=UIScheduler")
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseSection> CourseSections { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<TransferCourse> TransferCourses { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserSection> UserSections { get; set; }
        public virtual DbSet<ValidGened> ValidGeneds { get; set; }
        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(e => e.LastTaught)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CourseNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.LegacyCourseNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.TransferCourses)
                .WithOptional(e => e.Course)
                .HasForeignKey(e => e.countsAs);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.ValidGeneds)
                .WithOptional(e => e.Course1)
                .HasForeignKey(e => e.course);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.Session)
                .IsUnicode(false);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.StartTime)
                .HasPrecision(3);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.EndTime)
                .HasPrecision(3);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.Recurrence)
                .IsUnicode(false);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.Room)
                .IsUnicode(false);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.Building)
                .IsUnicode(false);

            modelBuilder.Entity<CourseSection>()
                .Property(e => e.MandatoryGroup)
                .IsUnicode(false);

            modelBuilder.Entity<Instructor>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Instructor>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<Instructor>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Instructor>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Instructor>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Instructor>()
                .Property(e => e.HawkID)
                .IsUnicode(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.shortDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.legacyCode)
                .IsUnicode(false);

            modelBuilder.Entity<UserSection>()
                .Property(e => e.StartTime)
                .HasPrecision(3);

            modelBuilder.Entity<UserSection>()
                .Property(e => e.EndTime)
                .HasPrecision(3);

            modelBuilder.Entity<UserSection>()
                .Property(e => e.Recurence)
                .IsUnicode(false);

            modelBuilder.Entity<UserSection>()
                .Property(e => e.Session)
                .IsUnicode(false);

            modelBuilder.Entity<webpages_Roles>()
                .HasMany(e => e.UserProfiles)
                .WithMany(e => e.webpages_Roles)
                .Map(m => m.ToTable("webpages_UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));
        }
    }
}
