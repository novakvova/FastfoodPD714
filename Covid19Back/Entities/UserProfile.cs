using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Back.Entities
{
    [Table("tblUserProfiles")]
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public long Id { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
        [Required, StringLength(250)]
        public string Phone { get; set; }
        [Required, StringLength(250)]
        public string Firstname { get; set; }
        [Required, StringLength(250)]
        public string Lastname { get; set; }

        public virtual DbUser User { get; set; }

    }
}
