using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Enum;

namespace Minimarket.Infrastructure.Data;

public partial class MinimarketContextTemp : DbContext
{
    public MinimarketContextTemp()
    {
    }

    public MinimarketContextTemp(DbContextOptions<MinimarketContextTemp> options)
        : base(options)
    {
    }

    public virtual DbSet<Security> Securities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MATEOQAYLAS;Database=MinimarketDB;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
