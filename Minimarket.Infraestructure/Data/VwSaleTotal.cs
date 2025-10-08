using System;
using System.Collections.Generic;

namespace Minimarket.Infraestructure.Data;

public partial class VwSaleTotal
{
    public int SaleId { get; set; }

    public string CustomerName { get; set; } = null!;

    public int? CustomerId { get; set; }

    public DateTime Date { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public bool IsDone { get; set; }

    public decimal TotalAmount { get; set; }
}
