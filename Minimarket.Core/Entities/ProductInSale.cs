using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Minimarket.Core.Data.Entities;

[PrimaryKey("IdSale", "IdProduct")]
[Table("ProductInSale")]
public partial class ProductInSale
{
    [Key]
    public int IdSale { get; set; }

    [Key]
    public int IdProduct { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("IdProduct")]
    [InverseProperty("ProductInSales")]
    public virtual Product IdProductNavigation { get; set; } = null!;

    [ForeignKey("IdSale")]
    [InverseProperty("ProductInSales")]
    public virtual Sale IdSaleNavigation { get; set; } = null!;
}
