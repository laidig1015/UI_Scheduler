namespace UI_Scheduler_Tool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserProfile")]
    public partial class UserProfile
    {
        public UserProfile()
        {
            webpages_Roles = new HashSet<webpages_Roles>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(56)]
        public string UserName { get; set; }

        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }
}
