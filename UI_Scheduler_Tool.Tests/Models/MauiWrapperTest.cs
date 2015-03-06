using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Models;

namespace UI_Scheduler_Tool.Tests.WrapperTests
{
    [TestClass]
    public class MauiWrapperTest
    {
        [TestMethod]
        public void GetCourse()
        {

            string result = MauiWrapper.GetCourse("055:032");
            Assert.IsTrue(result.Contains("Digital Design"),
                          "Unable to locate 'Digital Design' in resulting GetCourse JSON");
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
    }
}
