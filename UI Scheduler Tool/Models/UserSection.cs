namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserSection")]
    public partial class UserSection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [StringLength(16)]
        public string Recurence { get; set; }

        [Required]
        [StringLength(16)]
        public string Session { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
