using DataLayer.Data.Consts;
using Microsoft.AspNetCore.Identity;


namespace DataLayer.Data.Seeds
{
    public static class Roles
    {
        public static async Task SeedAdmin(RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync(ConstsNames.Admin);
            if (role is null)
                await roleManager.CreateAsync(new IdentityRole(ConstsNames.Admin));
            else
                return;
        }
        public static async Task SeedSeller(RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync(ConstsNames.Seller);
            if (role is null)
                await roleManager.CreateAsync(new IdentityRole(ConstsNames.Seller));
            else
                return;
        }
        public static async Task SeedBuyer(RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync(ConstsNames.Buyer);
            if (role is null)
                await roleManager.CreateAsync(new IdentityRole(ConstsNames.Buyer));
            else
                return;
        }
    }
}
