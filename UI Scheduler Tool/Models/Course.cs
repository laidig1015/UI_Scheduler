using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

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
        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
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

        public static void AddIgnoreRepeats(IEnumerable<Course> courses, DataContext db)
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

        public static void BulkAddIgnoreRepeats(List<Course> courses, DataContext db)
        {
            List<Course> coursesInDb = db.Courses.Where(c => c.CourseNumber != null).ToList();
            List<string> courseNumbersInDb = coursesInDb.Select(c => c.CourseNumber).Distinct().ToList();

            //Make List of Course Numbers
            //foreach(Course c in coursesInDb)
            //{
            //    courseNumbersInDb.Add(c.CourseNumber);
            //}
            bool test = courseNumbersInDb.Contains("akldfj;akljfk;ladjfk;laj");

            //Clear out Duplicates
            foreach (Course c in courses)
            {
                test = courseNumbersInDb.Contains(c.CourseNumber);
                if (courseNumbersInDb.Contains(c.CourseNumber))
                {
                    //courses.Remove(c);
                }
                else
                {
                    try
                    {
                        db.Courses.Add(c);
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }
            db.SaveChanges();
            //Add to Database
        }

        public static Course GetCourse(DataContext db, Course course)
        {
            // from: http://stackoverflow.com/questions/5377049/entity-framework-avoiding-inserting-duplicates
            var courses = from c in db.Courses where c.CourseName.Equals(course.CourseNumber) select c;

            if (courses.Count() > 0)
            {
                return courses.First();
            }

            // TODO: pull from cache

            db.Courses.Add(course);
            return course;
        }
    }
}