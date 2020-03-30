using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Back.DTO;
using Covid19Back.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Back.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult GetAll()
        {
            var model = _context.Products.Select(p => new ProductDTO
            {
                title = p.Name,
                price = p.Price.ToString(),
                url = p.Image
            }).ToList();


            //List<ProductDTO> model = new List<ProductDTO>() {
            //    new ProductDTO{
            //        title = "Salo",
            //        price = "99999",
            //        url = "http://10.0.2.2/android/salo1.jfif"
            //    },
            //    new ProductDTO{
            //        title = "Чебуреки",
            //        price = "15грн.",
            //        url = "http://10.0.2.2/android/2.jpg"
            //    }
            //};
            return Ok(model);
        }

    }
}