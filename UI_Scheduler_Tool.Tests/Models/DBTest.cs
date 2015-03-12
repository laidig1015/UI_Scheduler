using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Models;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace UI_Scheduler_Tool.Tests.Models
{
    [TestClass]
    public class DBTest
    {
        [TestMethod]
        public void AddNewCourse()
        {
            try
            {
                using (var db = new UIContext())
                {
                    Course course = new Course()
                    {
                        CourseName = "Test Course New",
                        CatalogDescription = "A test course",
                        CourseNumber = "TEST:000",
                        LegacyCourseNumber = "000:000",
                        CreditHours = "0"
                    };
                    db.Courses.Add(course);
                    db.SaveChanges();
                }
            }
            catch(DbUpdateException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
