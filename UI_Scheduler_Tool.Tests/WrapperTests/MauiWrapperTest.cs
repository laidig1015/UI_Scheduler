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
            result = MauiWrapper.getCourse("055:032");
        }
    }
}
