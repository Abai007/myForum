using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Models
{
    public class ForumContext : IdentityDbContext<User>
    {
        public DbSet<User> ForumUsers { get; set; }
        public DbSet<ImageModel> ImageModels { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public ForumContext(DbContextOptions<ForumContext> options) : base(options)
        {
        }
    }
}
