﻿using System;
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

        public static Course GetCourse(DataContext db, Course course)
        {
            // from: http://stackoverflow.com/questions/5377049/entity-framework-avoiding-inserting-duplicates

            //var cachedCourse = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added)
            //        .Where(ose => ose.EntitySet == db.Courses)
            //        .Select(ose => ose.Entity)
            //        .Cast<Course>()
            //        .Where(c => c.CourseNumber.Equals(course.CourseNumber))
            //        .SingleOrDefault();
    //        inventoryItem = context.ObjectStateManager.GetObjectStateEntries(EntityState.Added)
    //.Where(ose => ose.EntitySet == context.InventoryItems.EntitySet)
    //.Select(ose => ose.Entity)
    //.Cast<InventoryItem>()
    //.Where(equalityPredicate.Compile())
    //.SingleOrDefault();

    //        if (inventoryItem != null)
    //        {
    //            return inventoryItem;
    //        }

            //var courses = from c in db.Courses where c.CourseName.Equals(course.CourseNumber) select c;
            //if (courses.Count() > 0)
            //{
            //    return courses.First();
            //}

            //var cachedCourses = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added)
            //    .Where(ose => ose.EntitySet == db.Courses.Local)
            //    .Select(ose => ose.Entity)
            //    .Cast<Course>().Where(c => c.CourseNumber.Equals(course.CourseNumber));
            //if(cachedCourses.Count() != 0)
            //{
            //    return cachedCourses.First();
            //}

            //var cachedCourses = db.ObjectStateManager.GetObjectStateEntries(EntityState.Added).
            //Where(ose => ose.EntitySet == db.Tags.EntitySet).
            //Select(ose => ose.Entity).
            //Cast<Course>().Where(c => c.CourseNumber.Equals(course.CourseName));
            //if (cachedCourses.Count() != 0)
            //{
            //    return cachedCourses.First();
            //}

            // TODO: pull from cache

            db.Courses.Add(course);
            return course;
        }
    }
}