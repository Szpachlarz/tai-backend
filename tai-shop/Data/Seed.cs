using Microsoft.AspNetCore.Identity;
using tai_shop.Models;

namespace tai_shop.Data
{
    public static class Seed
    {
        public static async Task SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            //if (!await roleManager.RoleExistsAsync("Admin"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Admin"));
            //}
            //if (!await roleManager.RoleExistsAsync("User"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("User"));
            //}

            // Seed Admin User
            if (await userManager.FindByEmailAsync("admin@sklep.pl") == null)
            {
                var adminUser = new AppUser
                {
                    FirstName = "Admin",
                    LastName = "Admiński",
                    UserName = "admin@sklep.pl",
                    Email = "admin@sklep.pl"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Regular User
            if (await userManager.FindByEmailAsync("user@sklep.pl") == null)
            {
                var regularUser = new AppUser
                {
                    FirstName = "User",
                    LastName = "Userski",
                    UserName = "user@sklep.pl",
                    Email = "user@sklep.pl"
                };

                var result = await userManager.CreateAsync(regularUser, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }
        }
    }
}
