using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public static class TimeUtilities
    {
        public static long UnixTimestamp(this DateTime time)
        {
            return (long)(time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public static long CurrentUnixTimestamp()
        {
            return DateTime.UtcNow.UnixTimestamp();
        }
    }
}