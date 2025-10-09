using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Entidades;

namespace Minimarket.Infraestructure.Data;

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

    public virtual DbSet<VwSaleTotal> VwSaleTotals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MATEOQAYLAS;Database=MinimarketDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07695A37E7");

            entity.ToTable("Product");

            entity.HasIndex(e => e.Stock, "IX_Product_Stock");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductBrand).HasMaxLength(150);
        });

        modelBuilder.Entity<ProductInSale>(entity =>
        {
            entity.HasKey(e => new { e.IdSale, e.IdProduct });

            entity.ToTable("ProductInSale");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductInSales)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductInSale_Product");

            entity.HasOne(d => d.IdSaleNavigation).WithMany(p => p.ProductInSales)
                .HasForeignKey(d => d.IdSale)
                .HasConstraintName("FK_ProductInSale_Sale");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sale__3214EC07E53C197A");

            entity.ToTable("Sale");

            entity.HasIndex(e => e.Date, "IX_Sale_Date");

            entity.Property(e => e.CustomerName).HasMaxLength(300);
            entity.Property(e => e.Date).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07FC20CE8A");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "IX_User_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Telephone).HasMaxLength(50);
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        modelBuilder.Entity<VwSaleTotal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwSaleTotals");

            entity.Property(e => e.CustomerName).HasMaxLength(300);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(38, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
