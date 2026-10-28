using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity.DataSeed
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var users = new AppUser()
                {
                    DisplayName = "Mohamed",
                    Email = "muhammedhusseny@gmail.com",
                    UserName = "mohamedhusseny",
                    PhoneNumber = "01110809368",
                };

                await _userManager.CreateAsync(users, "Mohamed123$");
            }

        }
       
    }
}
