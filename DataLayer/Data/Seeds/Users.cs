using BusinessLogicLayer.Models;
using DataLayer.Data.Consts;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Data.Seeds
{
    public static class Users
    {
        public static async Task SeedAdmin(UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync("Admin");
            if (user is null)
            {
                var User = new User()
                {
                    UserName = "Admin",
                    Role = ConstsNames.Admin,
                };
                await userManager.CreateAsync(User, "Ba$$am3324");
                await userManager.AddToRolesAsync(User, new List<string>() { ConstsNames.Admin, ConstsNames.Seller, ConstsNames.Buyer });
            }
            else
                return;
        }
    }
}
