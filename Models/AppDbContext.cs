using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApi_app.Interfaces;

namespace WebApi_app.Models
{
    public class AppDbContext : DbContext , IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }


    }


}
