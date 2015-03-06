using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Models;
using System.Linq;

namespace UI_Scheduler_Tool.Tests.Models
{
    [TestClass]
    public class DBTest
    {
        [TestMethod]
        public void AddNewCourse()
        {
            using(var db = new UIowaScheduler())
            {
                Course course = new Course()
                {
                    CourseName = "Test Course",
                    CatalogDescription = "A test course",
                    CourseNumber = "TEST:000",
                    LegacyCourseNumber = "000:000",
                    CreditHours = "0"
                };
                db.Courses.Add(course);
                db.SaveChanges();
            }
        }
    }
}
