using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using UI_Scheduler_Tool.Models.Extensions;
using System.Data.Entity.Migrations;

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
        public string CatalogDescription { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        [Index]
        public string CourseNumber { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string CreditHours { get; set; }

        public bool IsOfferedInFall { get; set; }
        public bool IsOfferedInSpring { get; set; }

        public int LastTaughtID { get; set; }

        public virtual ICollection<PreqEdge> Parents { get; set; }
        public virtual ICollection<PreqEdge> Children { get; set; }

        public Course()
        {
            Parents = new HashSet<PreqEdge>();
            Children = new HashSet<PreqEdge>();
            IsOfferedInFall = true;
            IsOfferedInSpring = true;
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

        public void GetSubjectAndNumber(out string subject, out string number)
        {
            String[] parts = CourseNumber.Split(':');
            subject = parts[0];
            number = parts[1];
        }

        public static void AddIgnoreRepeats(DataContext db, IEnumerable<Course> courses)
        {
            // from: http://stackoverflow.com/questions/18113418/ignore-duplicate-key-insert-with-entity-framework
            var newCourses = courses.Select(c => c.CourseNumber).Distinct().ToArray();
            var coursesInDb = db.Courses.Where(c => newCourses.Contains(c.CourseNumber))
                                        .Select(c => c.CourseNumber).ToArray();
            var coursesNotInDb = courses.Where(c => !coursesInDb.Contains(c.CourseNumber));
            var list = coursesNotInDb.ToList();
            foreach (Course c in coursesNotInDb)
            {
                db.Courses.Add(c);
            }
            db.SaveChanges();
        }

        public Course Get(DataContext db)
        {
            return db.Courses.UniqueWhere(this, c => c.CourseNumber.Equals(CourseNumber));
        }

        public Course Add(DataContext db)
        {
            Course course = Get(db);
            db.Courses.AddOrUpdate(course);
            return course;
        }
    }
}