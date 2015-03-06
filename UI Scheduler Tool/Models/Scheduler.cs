using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public static class Scheduler
    {
        public static SchedulerNode[] SortByFinish(SchedulerNode[] nodes)
        {
            return nodes.OrderBy(n => n.Finish).ToArray();
        }

        public static SchedulerNode[] SortByStart(SchedulerNode[] nodes)
        {
            return nodes.OrderBy(n => n.Start).ToArray();
        }
    }
}