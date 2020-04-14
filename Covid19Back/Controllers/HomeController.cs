using System;
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
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        //private readonly IHostingEnvironment _env;
        public HomeController(ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //_env = env;
        }


        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            long id = long.Parse(User.Claims.ToList()[0].Value);
            string domain = (string)_configuration.GetValue<string>("BackendDomain");
            var user = _context.Users.
                Select(u => new
                {
                    u.Id,
                    u.Email,
                    Image = $"{domain}android/{u.UserProfile.Image}",
                    Name = $"{u.UserProfile.Lastname} {u.UserProfile.Firstname}",
                    u.UserProfile.Phone
                })
                .SingleOrDefault(x => x.Id == id);

            return Ok(user);

        }
    }
}
