using Swashbuckle.AspNetCore.Annotations;
using Minimarket.Core.QueryFilters;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Filtros y parámetros para consultar la cantidad total por venta y montos (paginado).")]
    public class ProductQuantityBySalePagination : PaginationQueryFilter
    {
        [SwaggerSchema("Identificador de la venta (opcional).", Nullable = true)]
        public int? IdSale { get; set; }

        [SwaggerSchema("Total de unidades vendidas en la venta (opcional).", Nullable = true)]
        public int? TotalUnitsSold { get; set; }

        [SwaggerSchema("Importe total de la venta (opcional).", Nullable = true, Format = "decimal")]
        public decimal? SaleTotalAmount { get; set; }
    }
}
