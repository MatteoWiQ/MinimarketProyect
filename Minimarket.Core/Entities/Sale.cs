using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Entities;

namespace Minimarket.Core.Data.Entities;

[Table("Sale")]
[Index("Date", Name = "IX_Sale_Date")]
public partial class Sale : BaseEntity
{
    //[Key]
    //public int Id { get; set; }

    [StringLength(300)]
    public string CustomerName { get; set; } = null!;

    public int? CustomerId { get; set; }

    public DateTime Date { get; set; }

    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    public bool IsDone { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Sales")]
    public virtual User? Customer { get; set; }

    [InverseProperty("IdSaleNavigation")]
    public virtual ICollection<ProductInSale> ProductInSales { get; set; } = new List<ProductInSale>();
}
