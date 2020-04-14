using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Back.DTO
{
    public class ProductDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string price { get; set; }
    }
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Вкажіть назву")]
        public string title { get; set; }
        [Required(ErrorMessage = "Вкажіть ціну")]
        public string price { get; set; }
        [Required(ErrorMessage = "Вкажіть фото")]
        public string imageBase64 { get; set; }
    }
    public class ProductEditDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string price { get; set; }
        [Required]
        public string url { get; set; }
        public string imageBase64 { get; set; }
    }
}
