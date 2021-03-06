﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bogus;
using Covid19Back.DTO;
using Covid19Back.Entities;
using Covid19Back.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Covid19Back.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public ProductsController(ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
        }
        public IActionResult GetAll()
        {
            string domain = (string)_configuration.GetValue<string>("BackendDomain");
            var model = _context.Products.Select(p => new ProductDTO
            {
                id=p.Id,
                title = p.Name,
                price = p.Price.ToString(),
                url = $"{domain}android/{p.Image}"
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
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromRoute] int id)
        {
            var item = _context.Products.SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                string domain = (string)_configuration.GetValue<string>("BackendDomain");
                ProductEditDTO product = new ProductEditDTO()
                {
                    Id = item.Id,
                    price = item.Price.ToString(),
                    title = item.Name,
                    url = $"{domain}android/{item.Image}"
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
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromBody]ProductEditDTO model)
        {
            var item = _context.Products.SingleOrDefault(x => x.Id == model.Id);
            if (item != null)
            {
                if (model.imageBase64 != null)
                {
                    var imageName = item.Image;
                    string savePath = _env.ContentRootPath;
                    string folderImage = "images";
                    savePath = Path.Combine(savePath, folderImage);
                    savePath = Path.Combine(savePath, imageName);
                    try
                    {
                        byte[] byteBuffer = Convert.FromBase64String(model.imageBase64);
                        using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
                        {
                            memoryStream.Position = 0;
                            using (Image imgReturn = Image.FromStream(memoryStream))
                            {
                                memoryStream.Close();
                                byteBuffer = null;
                                var bmp = new Bitmap(imgReturn);
                                bmp.Save(savePath, ImageFormat.Jpeg);
                            }
                        }
                    }
                    catch
                    {
                        return BadRequest(new
                        {
                            invalid = "Помилка обробки фото"
                        });
                    }
                }
                double price = 0;
                bool successfullyParsed = double.TryParse(model.price, out price);
                if (successfullyParsed)
                {
                    item.Price = price;
                    item.Name = model.title;

                    //_context.Entry(item).State = EntityState.Modified;
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
        [RequestSizeLimit(100 * 1024 * 1024)]     // set the maximum file size limit to 100 MB
        public IActionResult Create([FromBody]ProductCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errrors = CustomValidator.GetErrorsByModel(ModelState);
                return BadRequest(errrors);
            }
            double price = 0;
            bool successfullyParsed = double.TryParse(model.price, out price);
            if (successfullyParsed)
            {
                var imageName = Path.GetRandomFileName() + ".jpg";
                string savePath = _env.ContentRootPath;
                string folderImage = "images";
                savePath = Path.Combine(savePath, folderImage);
                savePath = Path.Combine(savePath, imageName);
                //using (FileStream fs = new FileStream(savePath, FileMode.Create))
                //{
                //    byte[] byteBuffer = Convert.FromBase64String(model.imageBase64);
                //    fs.Write(byteBuffer);
                //}

                try
                {
                    byte[] byteBuffer = Convert.FromBase64String(model.imageBase64);
                    using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
                    {
                        memoryStream.Position = 0;
                        using (Image imgReturn = Image.FromStream(memoryStream))
                        {
                            memoryStream.Close();
                            byteBuffer = null;
                            var bmp = new Bitmap(imgReturn);
                            bmp.Save(savePath, ImageFormat.Jpeg);
                        }
                    }
                }
                catch 
                {
                    return BadRequest(new
                    {
                        invalid = "Помилка обробки фото"
                    });
                }
                var faker = new Faker();
                Product product = new Product
                {
                    Name = model.title,
                    Image = imageName,
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
            else
            {
                return BadRequest(new
                {
                    invalid = "Формат ціни хх.хх"
                });
            }

        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var p = _context.Products.SingleOrDefault(x => x.Id == id);
            if (p != null)
            {
                var imageName = p.Image;
                string savePath = _env.ContentRootPath;
                string folderImage = "images";
                savePath = Path.Combine(savePath, folderImage);
                savePath = Path.Combine(savePath, imageName);
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }
                _context.Products.Remove(p);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new
                {
                    invalid = "Такого продукта немає!"
                });
            }


        }
    }
}