
using Microsoft.EntityFrameworkCore;
using ShopSphere.Api.Models;

namespace ShopSphere.Api.Data;

public class ShopSphereDbContext : DbContext
{
    public ShopSphereDbContext(
        DbContextOptions<ShopSphereDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(2000);

            entity.Property(p => p.SKU)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(p => p.SKU)
                .IsUnique();

            entity.Property(p => p.Price)
                .HasPrecision(18, 2);

            entity.Property(p => p.CreatedUtc)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Accessories" },
            new Category { Id = 3, Name = "Fashion" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Wireless Headphones",
                Description = "Wireless headphones for everyday listening.",
                SKU = "ELEC-001",
                Price = 2499.00m,
                IsActive = true,
                CategoryId = 1,
                CreatedUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Name = "Smart Watch",
                Description = "A smart watch for everyday activities.",
                SKU = "ELEC-002",
                Price = 3999.00m,
                IsActive = true,
                CategoryId = 1,
                CreatedUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 3,
                Name = "Everyday Backpack",
                Description = "A practical backpack for daily use.",
                SKU = "ACC-001",
                Price = 1499.00m,
                IsActive = true,
                CategoryId = 2,
                CreatedUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 4,
                Name = "Running Shoes",
                Description = "Comfortable shoes for running and training.",
                SKU = "FASH-001",
                Price = 2999.00m,
                IsActive = true,
                CategoryId = 3,
                CreatedUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
