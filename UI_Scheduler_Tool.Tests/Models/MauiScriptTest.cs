using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Maui;

namespace UI_Scheduler_Tool.Tests.Models
{
    [TestClass]
    public class MauiScriptTest
    {
        [TestMethod]
        public void PopulateCollegeCourses()
        {
            bool success = MauiScripts.PopulateCourseFromCollege("ECE");
            Assert.IsTrue(success, "Unable to populate db from maui");
        }
    }
}
