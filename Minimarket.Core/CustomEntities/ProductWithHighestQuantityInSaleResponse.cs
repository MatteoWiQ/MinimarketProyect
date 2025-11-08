using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Detalle de la línea de venta con mayor cantidad en una venta (producto con más unidades en esa venta).")]
    public class ProductWithHighestQuantityInSaleResponse
    {
        [SwaggerSchema("Identificador de la venta.")]
        public int IdSale { get; set; }

        [SwaggerSchema("Identificador del producto.")]
        public int IdProduct { get; set; }

        [SwaggerSchema("Nombre del producto.")]
        public string ProductName { get; set; }

        [SwaggerSchema("Cantidad vendida de ese producto en la venta.")]
        public int Quantity { get; set; }

        [SwaggerSchema("Precio unitario aplicado.", Format = "decimal")]
        public decimal UnitPrice { get; set; }

        [SwaggerSchema("Importe total de la línea (Quantity * UnitPrice).", Format = "decimal")]
        public decimal LineTotal { get; set; }

        [SwaggerSchema("Nombre del cliente de la venta.")]
        public string CustomerName { get; set; }

        [SwaggerSchema("Fecha de la venta.", Format = "date-time")]
        public DateTime Date { get; set; }
    }
}
