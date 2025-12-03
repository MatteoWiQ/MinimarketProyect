using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Data;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Entities;
using Minimarket.Core.Enum;
namespace Minimarket.Infrastructure.Data.Context;

public partial class MinimarketContext : DbContext
{
    public MinimarketContext()
    {
    }

    public MinimarketContext(DbContextOptions<MinimarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductInSale> ProductInSales { get; set; }
    public virtual DbSet<Sale> Sales { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Security> Securities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback para herramientas de diseño. Preferible: establecer la variable de entorno
            // "ConnectionStrings__ConnectionSqlServer" o usar un DesignTimeDbContextFactory para migraciones.
            var conn = Environment.GetEnvironmentVariable("ConnectionStrings__ConnectionSqlServer")
                       ?? "Server=MATEOQAYLAS;Database=MinimarketDB;Trusted_Connection=true;TrustServerCertificate=true;";
            optionsBuilder.UseSqlServer(conn);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07695A37E7");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ProductInSale>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductInSales)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductInSale_Product");

            entity.HasOne(d => d.IdSaleNavigation).WithMany(p => p.ProductInSales).HasConstraintName("FK_ProductInSale_Sale");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sale__3214EC07E53C197A");

            entity.Property(e => e.Date).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07FC20CE8A");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasIndex(e => e.Email, "IX_User_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
        modelBuilder.Entity<Security>(entity =>
        {
            entity.ToTable("Security");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasConversion(
                    x => x.ToString(),
                    x => (RoleType)Enum.Parse(typeof(RoleType), x)
            );

        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
