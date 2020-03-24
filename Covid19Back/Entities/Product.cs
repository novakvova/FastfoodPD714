using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Back.Entities
{
    [Table("tblProducts")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(7,2)")]
        public double Price { get; set; }

        [Required, StringLength(maximumLength: 4000)]
        public string Description { get; set; }

        [Required, StringLength(maximumLength: 250)]
        public string Name { get; set; }

        [Required, StringLength(maximumLength: 250)]
        public string Image { get; set; }
    }
}
