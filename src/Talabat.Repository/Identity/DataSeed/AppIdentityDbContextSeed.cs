using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, IConfiguration? configuration = null)
        {
            if (!userManager.Users.Any())
            {
                var displayName = configuration?["SeedUser:DisplayName"] ?? "Talabat Admin";
                var email = configuration?["SeedUser:Email"] ?? "admin@talabat.dev";
                var userName = configuration?["SeedUser:UserName"] ?? "admin";
                var phoneNumber = configuration?["SeedUser:PhoneNumber"] ?? "1234567890";
                var password = configuration?["SeedUser:Password"] ?? "Admin@123456";

                var user = new AppUser
                {
                    DisplayName = displayName,
                    Email = email,
                    UserName = userName,
                    PhoneNumber = phoneNumber,
                };

                await userManager.CreateAsync(user, password);
            }
        }
    }
}
