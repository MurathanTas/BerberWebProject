using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerberWeb.DataAccess
{
    public class SeeData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // Rolleri ve kullanıcıları oluşturmak için UserManager ve RoleManager alınır
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Rolleri oluştur
            string[] roles = { "Admin", "Personel", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role }); // AppRole oluşturuluyor
                }
            }

            // İki admin kullanıcı tanımla
            string adminEmail1 = "g221210048@sakarya.edu.tr";
            string adminEmail2 = "g221210084@sakarya.edu.tr";
            string adminPassword = "sau"; // Güçlü bir şifre kullanın

            if (userManager.Users.All(u => u.Email != adminEmail1))
            {
                var admin1 = new AppUser
                {
                    UserName = adminEmail1,
                    FirstName = "Murathan",
                    LastName = "Taş",
                    Email = adminEmail1,
                    EmailConfirmed = true
                };

                var result1 = await userManager.CreateAsync(admin1, adminPassword);
                if (result1.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin1, "Admin");
                }
            }

            if (userManager.Users.All(u => u.Email != adminEmail2))
            {
                var admin2 = new AppUser
                {
                    UserName = adminEmail2,
                    FirstName = "Buğra",
                    LastName = "Akşit",
                    Email = adminEmail2,
                    EmailConfirmed = true
                };

                var result2 = await userManager.CreateAsync(admin2, adminPassword);
                if (result2.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin2, "Admin");
                }
            }
        }
    }
}
