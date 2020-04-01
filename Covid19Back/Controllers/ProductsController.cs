using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Covid19Back.DTO;
using Covid19Back.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Covid19Back.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    //[Authorize]
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
        [HttpGet("edit/{id}")]
        //[Authorize(Roles ="Admin")]
        public IActionResult GetByIdProductForEdit([FromRoute] int id)
        {
            var item = _context.Products.SingleOrDefault(x => x.Id == id);
            if(item!=null)
            {
                ProductEditDTO product = new ProductEditDTO()
                {
                    Id = item.Id, price = item.Price.ToString(), title = item.Name
                };
                return Ok(product);
            }
            else
            {
                return BadRequest(new
                {
                    invalid = "Не знайдено по даному id"
                });
            }
        }
        [HttpPost("edit")]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateByIdProductForEdit([FromBody]ProductEditDTO model)
        {
            var item = _context.Products.SingleOrDefault(x => x.Id == model.Id);
            if (item != null)
            {
                item.Name = model.title;
                double price = 0;
                bool successfullyParsed = double.TryParse(model.price, out price);
                if (successfullyParsed)
                {
                    item.Price = price;
                    _context.Entry(item).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        invalid = "Не вірний тип данних"
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    invalid = "Не знайдено по даному id"
                });
            }
        }
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody]ProductCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    invalid = "Не валідна модель"
                });
            }
            var faker = new Faker();
            Product product = new Product
            {
                Name = model.title,
                Image = faker.Image.PicsumUrl(400, 400, false, false, null),
                Price = Double.Parse(model.price),
                Description = "Капець"
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(
            new
            {
                id = product.Id
            });
        }
    }
}