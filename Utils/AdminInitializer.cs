using Microsoft.AspNetCore.Identity;
using MyForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Utils
{
    public class AdminInitializer
    {
        public static async Task SeedAdminUser(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager)
        {
            var roles = new[] { "user" };
            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role) is null)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
