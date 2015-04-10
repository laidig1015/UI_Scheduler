using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Maui;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace UI_Scheduler_Tool.Tests.WrapperTests
{
    [TestClass]
    public class MauiWrapperTest
    {
        [TestMethod]
        public void GetCourse()
        {
            string result = MauiWrapper.GetCourse("055:032");
            MauiCourse course = new JavaScriptSerializer().Deserialize<MauiCourse>(result);
            Assert.IsTrue(course.title.Contains("Digital Design"),
                          "Unable to locate 'Digital Design' in resulting GetCourse JSON");
        }

        [TestMethod]
        public void GetAllECE()
        {
            string result = MauiWrapper.GetCourse("ECE");
            var courses = new JavaScriptSerializer().Deserialize<List<MauiCourse>>(result);
            Assert.IsTrue(courses.Count > 1,
                          "Unable to find all ECE courses in result Course List");
        }

        [TestMethod]
        public void GetMinors()
        {
            string result = MauiWrapper.GetMinors();
            Assert.IsTrue(result.Contains("Aerospace Studies"),
                          "Unable to locate 'Aerospace Studies' in resulting GetMinors JSON");
        }

        [TestMethod]
        public void GetProgramsofStudyByNatKey()
        {
            string result = MauiWrapper.GetProgramsOfStudyByNatKey("R");
            Assert.IsTrue(result.Contains("Aerospace Studies"),
                          "Unable to locate 'Aerospace Studies' in resulting GetProgramsofStudyByNatKey JSON");
        }

        [TestMethod]
        public void GetProgramofStudyByID()
        {
            string result = MauiWrapper.GetProgramOfStudyByID("305");
            Assert.IsTrue(result.Contains("Communication Studies"),
                          "Unable to locate 'Communication Studies' in resulting GetProgramofStudyByID JSON");
        }

        [TestMethod]
        public void GetCurrentSession()
        {
            string result = MauiWrapper.GetCurrentSession();
            Assert.IsTrue(result.Contains("startDate"),
                          "Unable to locate 'startDate' in resulting GetCurrentSession JSON");
        }

        [TestMethod]
        public void GetAllSessions()
        {
            string result = MauiWrapper.GetAllSessions();
            Assert.IsTrue(result.Contains("startDate"),
                          "Unable to locatel 'startDate' in resulting GetAllSessions JSON");
        }

        [TestMethod]
        public void GetSection()
        {
            // TODO: maybe deserialize
            string result = MauiWrapper.GetSections(59, "CS", "3330");
            Assert.IsTrue(result.Contains("Algorithms"),
                          "Unable to locate 'Algorithms' in resulting GetSection JSON");
            MauiCourse dummy = new MauiCourse()
            {
                title = "Algorithms",
                catalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                lastTaught = "Spring 2015",
                lastTaughtId= 59,
                lastTaughtCode= "20148",
                courseNumber= "CS:3330",
                legacyCourseNumber= "22C:031",
                creditHours= "3"
            };
            var sections = MauiSection.Get(dummy);
        }

        [TestMethod]
        public void createPrerequesites()
        {
            MauiCourse dummy = new MauiCourse()
            {
                title = "Algorithms",
                catalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                lastTaught = "Spring 2015",
                lastTaughtId = 59,
                lastTaughtCode = "20148",
                courseNumber = "CS:3330",
                legacyCourseNumber = "22C:031",
                creditHours = "3"
            };
            MauiSection.createPrerequesties(dummy);

            //https://api.maui.uiowa.edu/maui/api/pub/registrar/sections?json={sessionId: 59, courseSubject: 'CS', courseNumber: '3330'}&pageStart=0&pageSize=2147483647&
            //string college = 'ECE';
            
            //MauiScripts.PopulateCourseFromCollege("ECE");

        }
    }
}
