using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Resumen con cantidades por cada venta (count de productos distintos, unidades totales y total monetario).")]
    public class ProductQuantityBySaleResponse
    {
        [SwaggerSchema("Identificador de la venta.")]
        public int IdSale { get; set; }

        [SwaggerSchema("Cantidad de productos distintos en la venta.")]
        public int DistinctProductsCount { get; set; }

        [SwaggerSchema("Unidades totales vendidas en la venta (suma de cantidades).")]
        public int TotalUnitsSold { get; set; }

        [SwaggerSchema("Importe total de la venta (suma de lineas).", Format = "decimal")]
        public decimal SaleTotalAmount { get; set; }
    }
}
