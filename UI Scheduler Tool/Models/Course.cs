namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        public Course()
        {
            CourseSections = new HashSet<CourseSection>();
            TransferCourses = new HashSet<TransferCourse>();
            ValidGeneds = new HashSet<ValidGened>();
        }

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

        public virtual ICollection<CourseSection> CourseSections { get; set; }

        public virtual ICollection<TransferCourse> TransferCourses { get; set; }

        public virtual ICollection<ValidGened> ValidGeneds { get; set; }
    }
}
