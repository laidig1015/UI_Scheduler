namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using UI_Scheduler_Tool.Maui;

    [Table("Course")]
    public partial class Course
    {
        #region Fields
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string CourseName { get; set; }

        [Column(TypeName = "ntext")]
        public string CatalogDescription { get; set; }

        [StringLength(16)]
        public string LastTaught { get; set; }

        [Required]
        [StringLength(16)]
        public string CourseNumber { get; set; }

        [Required]
        [StringLength(16)]
        public string LegacyCourseNumber { get; set; }

        [Required]
        [StringLength(16)]
        public string CreditHours { get; set; }

        public int LastTaughtID { get; set; }

        public virtual ICollection<CourseSection> CourseSections { get; set; }

        public virtual ICollection<TransferCourse> TransferCourses { get; set; }

        public virtual ICollection<ValidGened> ValidGeneds { get; set; }
        #endregion

        public Course()
        {
            CourseSections = new HashSet<CourseSection>();
            TransferCourses = new HashSet<TransferCourse>();
            ValidGeneds = new HashSet<ValidGened>();
        }

        public Course(MauiCourse mauiCourse)
        {
            CourseName = mauiCourse.title;
            CatalogDescription = mauiCourse.catalogDescription;
            LastTaught = mauiCourse.lastTaught;
            CourseNumber = mauiCourse.courseNumber;
            LegacyCourseNumber = mauiCourse.legacyCourseNumber;
            CreditHours = mauiCourse.creditHours;
            LastTaughtID = mauiCourse.lastTaughtId;
        }

        public static explicit operator Course(MauiCourse mauiCourse)
        {
            return new Course(mauiCourse);
        }

        public static List<Course> FromMauiCourses(List<MauiCourse> mauiCourses)
        {
            return mauiCourses == null ?
                new List<Course>() :
                mauiCourses.Select(mc => (Course)mc).ToList();
        }
    }
}
