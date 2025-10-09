using System;
using System.Collections.Generic;

namespace Minimarket.Core.Entidades;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ProductBrand { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ProductInSale> ProductInSales { get; set; } = new List<ProductInSale>();
}
