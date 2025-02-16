using Microsoft.AspNetCore.Identity;
using tai_shop.Models;

namespace tai_shop.Data
{
    public static class Seed
    {
        public static async Task SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Admin User
            var adminEmail = "admin@sklep.pl";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
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
                    await userManager.AddToRoleAsync(adminUser, "User");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "User"))
                {
                    await userManager.AddToRoleAsync(adminUser, "User");
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
