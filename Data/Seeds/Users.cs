using Microsoft.AspNetCore.Identity;
using TheTask.Data.Consts;
using TheTask.Models;

namespace TheTask.Data.Seeds
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
