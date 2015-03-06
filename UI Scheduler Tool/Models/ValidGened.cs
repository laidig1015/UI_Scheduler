namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ValidGened
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string collegeValidFor { get; set; }

        [Required]
        [StringLength(200)]
        public string typeOfGened { get; set; }

        public int? course { get; set; }

        public int? creditHours { get; set; }

        public virtual Course Course1 { get; set; }
    }
}
