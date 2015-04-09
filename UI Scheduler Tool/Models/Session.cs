using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public partial class Session
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ShortDescription { get; set; }
        public string LegacyCode { get; set; }
        public int? MauiID { get; set; }
    }
}