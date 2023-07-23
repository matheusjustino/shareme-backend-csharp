namespace shareme_backend.Data;

using Microsoft.EntityFrameworkCore;
using shareme_backend.Models;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Username).IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Email).IsUnique();
    }
}