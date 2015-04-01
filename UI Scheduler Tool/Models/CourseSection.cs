namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CourseSection")]
    public partial class CourseSection
    {
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string Session { get; set; }

        [Column(TypeName = "date")]
        public DateTime BeginDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(16)]
        public string Type { get; set; }

        public int Hours { get; set; }

        public int? MaxEnroll { get; set; }

        public int? CurrentEnroll { get; set; }

        public int? MaxUnreservedEnroll { get; set; }

        public int? CurrentUnreservedEnroll { get; set; }

        public bool? UnlimitedEnroll { get; set; }

        public bool? IsIndependentStudy { get; set; }

        [StringLength(16)]
        public string DeliveryMode { get; set; }

        [StringLength(16)]
        public string DeliveryTools { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        [StringLength(16)]
        public string Recurrence { get; set; }

        [StringLength(16)]
        public string Room { get; set; }

        [StringLength(16)]
        public string Building { get; set; }

        public bool? ArrangedTime { get; set; }

        public bool? ArrangedLocation { get; set; }

        public bool? Offsite { get; set; }

        [StringLength(64)]
        public string MandatoryGroup { get; set; }

        public int? CourseID { get; set; }

        public int? MAUISectionID { get; set; }

        public virtual Course Course { get; set; }

    }
}
