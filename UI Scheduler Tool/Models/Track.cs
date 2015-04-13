﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UI_Scheduler_Tool.Models
{
    public class Track
    {
        public int ID { get; set; }
        
        [Index]
        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string ShortName { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(64)]
        public string Name { get; set; }

        public virtual ICollection<Curriculum> Curricula { get; set; }
    }
}