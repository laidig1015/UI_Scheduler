using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Models;
using System.Linq;
using System.Data;
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
                using (var db = new DataContext())
                {
                    Course course = new Course()
                    {
                        CourseName = "Test Course New",
                        CatalogDescription = "A test course",
                        CourseNumber = "TEST:000",
                        CreditHours = "0",
                        LastTaughtID = 0
                    };
                    db.Courses.Add(course);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException due)
            {
                Console.WriteLine(due.Message);
            }
            catch(DataException de)
            {
                Console.WriteLine(de.Message);
            }
        }
    }
}
