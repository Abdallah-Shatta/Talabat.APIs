using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Respository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> usermanager)
        {
            if (usermanager.Users.Count() == 0)
            {
                var user = new AppUser
                {
                    DisplayName = "Abdallah shatta",
                    Email = "abdallah.shatta@mail.com",
                    UserName = "abdallah_shatta",
                    PhoneNumber = "01122334455"
                }; 
                await usermanager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
