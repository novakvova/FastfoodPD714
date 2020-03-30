using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Back.Entities
{
    public static class SeederDatabase
    {
        public static void SeedData(IServiceProvider services,
           IWebHostEnvironment env,
           IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<DbRole>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedUsers(manager, managerRole);
                SeedProduct(context);
            }
        }

        private static void SeedUsers(UserManager<DbUser> userManager,
            RoleManager<DbRole> roleManager)
        {
            var roleName = "Admin";
            if (roleManager.FindByNameAsync(roleName).Result == null)
            {
                var result = roleManager.CreateAsync(new DbRole
                {
                    Name = roleName
                }).Result;
            }

            if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
            {
                string email = "admin@gmail.com";
                var user = new DbUser
                {
                    Email = email,
                    UserName = email,
                    PhoneNumber = "+11(111)111-11-11"
                };
                var result = userManager.CreateAsync(user, "8Ki9x9-3of+s222").Result;
                result = userManager.AddToRoleAsync(user, roleName).Result;
            }
            if (userManager.FindByEmailAsync("novakvova@gmail.com").Result == null)
            {
                string email = "novakvova@gmail.com";
                var user = new DbUser
                {
                    Email = email,
                    UserName = email,
                    PhoneNumber = "+21(111)111-11-11"
                };
                var result = userManager.CreateAsync(user, "R2-=x*x1PxsE2219").Result;
                result = userManager.AddToRoleAsync(user, roleName).Result;
            }
        }
        private static void SeedProduct(ApplicationDbContext context)
        {
            var faker = new Faker();
            List<Product> products = new List<Product>();


            for (int i = 0; i < 30; i++)
            {
                products.Add(new Product { Name = faker.Commerce.ProductName(), 
                    Price = faker.Random.Double(1,1000), 
                    Description = faker.Commerce.Product(), 
                    Image = faker.Image.PicsumUrl(400,400,false,false,null)});
            }


            for (int i = 0; i < products.Count; i++)
            {
                if (context.Products.SingleOrDefault(r => r.Name == products[i].Name) == null)
                {
                    context.Products.Add(products[i]);
                    context.SaveChanges();
                }
            }

        }
    }
}
