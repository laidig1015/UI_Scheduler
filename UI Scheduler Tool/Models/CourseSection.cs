using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public partial class CourseSection
    {
        public int ID { get; set; }
        public string Session { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SectionType { get; set; }
        public string Hours { get; set; }
        public string MaxEnroll { get; set; }
        public int? CurrentEnroll { get; set; }
        public string MaxUnreservedEnroll { get; set; }
        public int? CurrentUnreservedEnroll { get; set; }
        public bool? UnlimitedEnroll { get; set; }
        public bool? IsIndependentStudy { get; set; }
        public string DeliveryMode { get; set; }
        public string DeliveryTools { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Recurrence { get; set; }
        public string Room { get; set; }
        public string Building { get; set; }
        public bool? ArrangedTime { get; set; }
        public bool? ArrangedLocation { get; set; }
        public bool? Offsite { get; set; }
        public string MandatoryGroup { get; set; }
        public int? CourseID { get; set; }
        public int? MAUISectionID { get; set; }

        public virtual Course Course { get; set; }
    }
}