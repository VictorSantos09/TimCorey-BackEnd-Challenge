using ChallengeCore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeCore.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(250);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(250);
            entity.Property(u => u.Nickname).IsRequired().HasMaxLength(100);
            entity.HasMany(u => u.Purchases)
              .WithOne(p => p.User)
              .HasForeignKey(p => p.UserId);
        });

        builder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.Name).IsRequired().HasMaxLength(250);
            entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.HasMany(p => p.PurchaseItems)
              .WithOne(pi => pi.Product)
              .HasForeignKey(pi => pi.ProductId);
        });

        builder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchases");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.PurchaseDate).IsRequired();
            entity.HasOne(p => p.User)
              .WithMany(u => u.Purchases)
              .HasForeignKey(p => p.UserId);
            entity.HasMany(p => p.Items)
              .WithOne(pi => pi.Purchase)
              .HasForeignKey(pi => pi.PurchaseId);
        });

        builder.Entity<PurchaseItem>(entity =>
        {
            entity.ToTable("PurchaseItems");
            entity.HasKey(pi => pi.Id);
            entity.Property(pi => pi.Id).ValueGeneratedOnAdd();
            entity.Property(pi => pi.Quantity).IsRequired();
            entity.HasOne(pi => pi.Purchase)
              .WithMany(p => p.Items)
              .HasForeignKey(pi => pi.PurchaseId);
            entity.HasOne(pi => pi.Product)
              .WithMany(p => p.PurchaseItems)
              .HasForeignKey(pi => pi.ProductId);
        });
    }
}
