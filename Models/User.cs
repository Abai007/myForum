using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
    }
}
