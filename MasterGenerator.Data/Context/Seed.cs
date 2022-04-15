using MasterGenerator.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Context
{
    public class Seed
    {
        public static async Task SeedRoles(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;
            if (await roleManager.Roles.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Admin"},
                new AppRole{Name = "CS User"},
                new AppRole{Name = "Customer User"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "kunal",
                Email = "kunal@curatedsourcing.com"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
        }
    }
}
