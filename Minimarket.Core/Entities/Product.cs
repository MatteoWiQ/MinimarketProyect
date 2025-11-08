using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Minimarket.Core.Data.Entities;

[Table("Product")]
[Index("Stock", Name = "IX_Product_Stock")]
[SwaggerSchema("Entidad Product que representa un producto en la base de datos.")]
public partial class Product : BaseEntity
{
    //[Key]
    //public int Id { get; set; }

    [StringLength(200)]
    [SwaggerSchema("Nombre del producto.")]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    [SwaggerSchema("Marca del producto (opcional).", Nullable = true)]
    public string? ProductBrand { get; set; }

    [StringLength(1000)]
    [SwaggerSchema("Descripción detallada del producto (opcional).", Nullable = true)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    [SwaggerSchema("Precio unitario del producto (dos decimales).", Format = "decimal")]
    public decimal Price { get; set; }

    [SwaggerSchema("Cantidad disponible en stock.")]
    public int Stock { get; set; }

    [SwaggerSchema("Indica si el producto está activo y disponible para la venta.")]
    public bool IsActive { get; set; }

    [SwaggerSchema("Fecha de creación del registro del producto.")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("IdProductNavigation")]
    [SwaggerSchema("Colección de líneas ProductInSale asociadas a este producto.", ReadOnly = true)]
    public virtual ICollection<ProductInSale> ProductInSales { get; set; } = new List<ProductInSale>();
}
