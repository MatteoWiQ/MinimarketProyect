using Swashbuckle.AspNetCore.Annotations;
using Minimarket.Core.QueryFilters;
using System;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Filtros y criterios para paginar y consultar el detalle de productos por venta.")]
    public class ProductInSaleDetailsPagination : PaginationQueryFilter
    {
        [SwaggerSchema("Identificador de la venta (opcional).", Nullable = true)]
        public int? IdSale { get; set; }

        [SwaggerSchema("Identificador del producto (opcional).", Nullable = true)]
        public int? IdProduct { get; set; }

        [SwaggerSchema("Nombre del producto (opcional).", Nullable = true)]
        public string? ProductName { get; set; }

        [SwaggerSchema("Marca del producto (opcional).", Nullable = true)]
        public string? ProductBrand { get; set; }

        [SwaggerSchema("Cantidad vendida del producto (opcional).", Nullable = true)]
        public int? Quantity { get; set; }

        [SwaggerSchema("Nombre del cliente (opcional).", Nullable = true)]
        public string? CustomerName { get; set; }

        [SwaggerSchema("Método de pago utilizado en la venta (opcional).", Nullable = true)]
        public string? PaymentMethod { get; set; }

        [SwaggerSchema("Indica si la venta está finalizada (opcional).", Nullable = true)]
        public bool? IsDone { get; set; }

        [SwaggerSchema("Fecha de la venta (opcional).", Nullable = true, Format = "date-time")]
        public DateTime? Date { get; set; }
    }
}
