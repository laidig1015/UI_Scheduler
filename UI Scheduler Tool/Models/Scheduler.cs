using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class Scheduler
    {
        public struct Node
        {
            public long Start;
            public long Finish;

            public Node(long start, long finish)
            {
                Start = start;
                Finish = finish;
            }

            public bool Overlaps(Node other)
            {
                return (Start >= other.Start && Start <= other.Finish)
                       || (Finish >= other.Start && Finish <= other.Finish);
            }

            public override string ToString()
            {
                return String.Format("{0:g} -> {1:g}", Start, Finish);
            }
        }

    }
}