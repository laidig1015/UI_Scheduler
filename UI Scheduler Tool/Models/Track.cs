using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UI_Scheduler_Tool.Models.Extensions;
using System.Data.Entity.Migrations;

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
        public virtual ICollection<EFA> EFAs { get; set; }

        public Track Get(DataContext db)
        {
            return db.Tracks.UniqueWhere(this, t => t.ShortName.Equals(ShortName));
        }

        public Track Add(DataContext db)
        {
            Track track = Get(db);
            db.Tracks.AddOrUpdate(track);
            return track;
        }
    }
}