using Microsoft.AspNetCore.Identity;

namespace tai_shop.Data
{
    public static class RoleSeed
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = roleName, NormalizedName = roleName.ToUpper() });
                }
            }
        }
    }
}
