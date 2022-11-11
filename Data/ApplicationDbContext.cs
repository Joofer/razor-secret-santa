using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using razor_secret_santa.Models;
using System.Reflection.Metadata.Ecma335;

namespace razor_secret_santa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<GiftModel> GiftModels { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}