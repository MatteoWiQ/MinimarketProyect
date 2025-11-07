using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Entities;

namespace Minimarket.Core.Data.Entities;

[Table("Product")]
[Index("Stock", Name = "IX_Product_Stock")]
public partial class Product : BaseEntity
{
    //[Key]
    //public int Id { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string? ProductBrand { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("IdProductNavigation")]
    public virtual ICollection<ProductInSale> ProductInSales { get; set; } = new List<ProductInSale>();
}
