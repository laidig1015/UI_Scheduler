using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public struct SchedulerNode
    {
        public long Start;
        public long Finish;

        public SchedulerNode(long start, long finish)
        {
            Start = start;
            Finish = finish;
        }

        public bool Overlaps(SchedulerNode other)
        {
            return (Start >= other.Start && Start <= other.Finish)// start betwen other start and finish
                   || (Finish >= other.Start && Finish <= other.Finish);// finish between other start and finish
        }

        public override string ToString()
        {
            return String.Format("{0} -> {1}", Start, Finish);
        }
    }
}