using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Models;

namespace UI_Scheduler_Tool.Tests.Models
{
    [TestClass]
    public class SchedulerTest
    {
        [TestMethod]
        public void NodeOverlap()
        {
            // nodes to compare
            SchedulerNode a, b;

            // overlaps in middle
            // a (Jan 1st 2015 @ 08:00AM - 10:00AM)
            // b (Jan 1st 2015 @ 09:00AM - 12:00PM)
            a = new SchedulerNode(new DateTime(2015, 1, 1, 8, 0, 0).UnixTimestamp(),
                                   new DateTime(2015, 1, 1, 10, 0, 0).UnixTimestamp());
            b = new SchedulerNode(new DateTime(2015, 1, 1, 9, 0, 0).UnixTimestamp(),
                                   new DateTime(2015, 1, 1, 12, 0, 0).UnixTimestamp());
            Assert.IsTrue(a.Overlaps(b), "The nodes A({0}) and B({1}) should overlap", a, b);

            // overlaps exactly
            // a (Jan 1st 2015 @ 8:00AM - 10:00AM)
            // b (Jan 1st 2015 @ 8:00AM - 10:00AM)
            a = new SchedulerNode(new DateTime(2015, 1, 1, 8, 0, 0).UnixTimestamp(),
                                   new DateTime(2015, 1, 1, 10, 0, 0).UnixTimestamp());
            b = new SchedulerNode(new DateTime(2015, 1, 1, 8, 0, 0).UnixTimestamp(),
                                   new DateTime(2015, 1, 1, 10, 0, 0).UnixTimestamp());
            Assert.IsTrue(a.Overlaps(b), "The nodes A({0}) and B({1}) should overlap", a, b);

            // shouldn't overlap
            // a (Jan 1st 2015 @ 08:00AM - 10:00AM)
            // b (Jan 1st 2015 @ 11:00AM - 1:00PM)
            a = new SchedulerNode(new DateTime(2015, 1, 1, 8, 0, 0).UnixTimestamp(),
                                   new DateTime(2015, 1, 1, 10, 0, 0).UnixTimestamp());
            b = new SchedulerNode(new DateTime(2015, 1, 1, 11, 0, 0).UnixTimestamp(),
                                   new DateTime(2015, 1, 1, 13, 0, 0).UnixTimestamp());
            Assert.IsFalse(a.Overlaps(b), "The nodes A({0}) and B({1}) should not overlap", a, b);
        }
    }
}
