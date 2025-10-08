using System;
using System.Collections.Generic;

namespace Minimarket.Infraestructure.Data;

public partial class ProductInSale
{
    public int IdSale { get; set; }

    public int IdProduct { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Sale IdSaleNavigation { get; set; } = null!;
}
