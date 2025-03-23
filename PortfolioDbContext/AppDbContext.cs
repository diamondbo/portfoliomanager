using Microsoft.EntityFrameworkCore;
using portfoliomanager.Models;

namespace portfoliomanager.PortFolioDbContext
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {
        }

        public DbSet<Userdb> Users { get; set; }
        public DbSet<Projectdb> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Userdb>().ToTable("Users");
            modelBuilder.Entity<Userdb>().HasKey(u => u.Id);
            modelBuilder.Entity<Userdb>()
            .HasIndex(u => u.Email)
            .IsUnique();
            modelBuilder.Entity<Projectdb>().ToTable("Projects");

        }
    }

}