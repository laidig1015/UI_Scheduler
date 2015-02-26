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
            result = MauiWrapper.GetMinors();
            result = MauiWrapper.GetProgramsOfStudyByNatKey("R");
            result = MauiWrapper.GetProgramOfStudyByProgramNatKey("ANTH");
            result = MauiWrapper.GetProgramOfStudyByID("305");
            Console.WriteLine();
        }
    }
}
