using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Models;

namespace UI_Scheduler_Tool.Tests.WrapperTests
{
    [TestClass]
    public class MauiWrapperTest
    {
        [TestMethod]
        public void getCourse()
        {
            string result;
            result = MauiWrapper.GetCourse("055:032");
            //result = MauiWrapper.GetMinors();
            Boolean contains_value = result.Contains("Digital Design");
            Assert.IsTrue(contains_value);
            //result = MauiWrapper.GetProgramsOfStudyByNatKey("R");
            //result = MauiWrapper.GetProgramOfStudyByProgramNatKey("ANTH");
            //result = MauiWrapper.GetProgramOfStudyByID("305");
        }

        [TestMethod]
        public void getMinors()
        {
            string result;
            result = MauiWrapper.GetMinors();
            Boolean contains_value = result.Contains("Aerospace Studies");
            Assert.IsTrue(contains_value);
        }

        [TestMethod]
        public void getProgramsofStudyByNatKey()
        {
            string result;
            result = MauiWrapper.GetProgramsOfStudyByNatKey("R");
            Boolean contains_value = result.Contains("Aerospace Studies");
            Assert.IsTrue(contains_value);
        }

        [TestMethod]
        public void getProgramofStudyByID()
        {
            string result;
            result = MauiWrapper.GetProgramOfStudyByID("305");
            Boolean contains_value = result.Contains("Communication Studies");
            Assert.IsTrue(contains_value);
        }

    }
}
