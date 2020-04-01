using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Back.DTO
{
    public class ProductDTO
    {
        public string title { get; set; }
        public string url { get; set; }
        public string price { get; set; }
    }
    public class ProductCreateDTO
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string price { get; set; }
    }
}
