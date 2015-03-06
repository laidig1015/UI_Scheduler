namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Session")]
    public partial class Session
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        [Required]
        [StringLength(16)]
        public string shortDescription { get; set; }

        [Required]
        [StringLength(16)]
        public string legacyCode { get; set; }

        public int? mauiID { get; set; }
    }
}
