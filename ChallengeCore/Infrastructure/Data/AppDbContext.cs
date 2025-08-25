using ChallengeCore.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace ChallengeCore.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Purchase> Purchases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>(entity =>
        {
            _ = entity.ToTable("Users");
            _ = entity.HasKey(u => u.Id);
            _ = entity.Property(u => u.Id).ValueGeneratedOnAdd();
            _ = entity.Property(u => u.Name).IsRequired().HasMaxLength(250);
            _ = entity.Property(u => u.Email).IsRequired().HasMaxLength(250);
            _ = entity.Property(u => u.Nickname).IsRequired().HasMaxLength(100);
            _ = entity.HasMany(u => u.Purchases)
              .WithOne(p => p.User)
              .HasForeignKey(p => p.UserId);
        });

        _ = modelBuilder.Entity<Product>(entity =>
        {
            _ = entity.ToTable("Products");
            _ = entity.HasKey(p => p.Id);
            _ = entity.Property(p => p.Id).ValueGeneratedOnAdd();
            _ = entity.Property(p => p.Name).IsRequired().HasMaxLength(250);
            _ = entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            _ = entity.HasMany(p => p.PurchaseItems)
              .WithOne(pi => pi.Product)
              .HasForeignKey(pi => pi.ProductId);
        });

        _ = modelBuilder.Entity<Purchase>(entity =>
        {
            _ = entity.ToTable("Purchases");
            _ = entity.HasKey(p => p.Id);
            _ = entity.Property(p => p.Id).ValueGeneratedOnAdd();
            _ = entity.Property(p => p.PurchaseDate).IsRequired();
            _ = entity.HasOne(p => p.User)
              .WithMany(u => u.Purchases)
              .HasForeignKey(p => p.UserId);
            _ = entity.HasMany(p => p.Items)
              .WithOne(pi => pi.Purchase)
              .HasForeignKey(pi => pi.PurchaseId);
        });

        _ = modelBuilder.Entity<PurchaseItem>(entity =>
        {
            _ = entity.ToTable("PurchaseItems");
            _ = entity.HasKey(pi => pi.Id);
            _ = entity.Property(pi => pi.Id).ValueGeneratedOnAdd();
            _ = entity.Property(pi => pi.Quantity).IsRequired();
            _ = entity.HasOne(pi => pi.Purchase)
              .WithMany(p => p.Items)
              .HasForeignKey(pi => pi.PurchaseId);
            _ = entity.HasOne(pi => pi.Product)
              .WithMany(p => p.PurchaseItems)
              .HasForeignKey(pi => pi.ProductId);
        });
    }
}
