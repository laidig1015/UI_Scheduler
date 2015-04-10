using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UI_Scheduler_Tool.Models
{
    public class Course
    {
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        public string CourseName { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string CatalogDescription { get; set; }

        [Required]
        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string CourseNumber { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string CreditHours { get; set; }

        public int Occurence { get; set; }
        public int LastTaughtID { get; set; }

        public virtual ICollection<PreqEdge> Parents { get; set; }
        public virtual ICollection<PreqEdge> Children { get; set; }

        public Course()
        {
            Parents = new HashSet<PreqEdge>();
            Children = new HashSet<PreqEdge>();
        }

        public string CourseSubject
        {
            get
            {
                return CourseNumber.Substring(0, CourseNumber.IndexOf(':'));
            }
        }

        public int CourseIdentity
        {
            get
            {
                int i = CourseNumber.IndexOf(':');
                return Int32.Parse(CourseNumber.Substring(i + 1, CourseNumber.Length - i));
            }
        }

        public int CreditHoursAsNumber
        {
            get
            {
                int hours;
                return Int32.TryParse(CreditHours, out hours) ? hours : 0;
            }
        }
    }
}