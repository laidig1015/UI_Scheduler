namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransferCourse")]
    public partial class TransferCourse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string courseName { get; set; }

        [Required]
        [StringLength(200)]
        public string collegeName { get; set; }

        public int? creditHours { get; set; }

        public int? countsAs { get; set; }

        public virtual Course Course { get; set; }
    }
}
