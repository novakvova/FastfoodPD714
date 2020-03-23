using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Back.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Back.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        public IActionResult GetAll()
        {
            List<ProductDTO> model = new List<ProductDTO>() {
            new ProductDTO{
                title = "Salo",
                price = "99999",
                url = "/android/salo1.jfif"


            }
            };
            return Ok(model);
        }
        
    }
}