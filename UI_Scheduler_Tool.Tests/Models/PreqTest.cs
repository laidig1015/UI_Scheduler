using System;
using System.Linq;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Models;
using System.IO;

namespace UI_Scheduler_Tool.Tests.Models
{
    [TestClass]
    public class PreqTest
    {
        [TestMethod]
        public void RootHasValidChildren()
        {
            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
                using (var db = new DataContext())
                {
                    Course root = db.Courses.Where(c => c.CourseName.Equals("RootRequired")).Single();
                    Assert.IsTrue(root.Children.Count() == 1, "RootRequired should have one child 'Easy'");
                }
            }
            catch (System.Data.DataException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [TestMethod]
        public void RootHasValidParents()
        {

        }
    }
}
