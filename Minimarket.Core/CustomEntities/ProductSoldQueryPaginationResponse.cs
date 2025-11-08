using Swashbuckle.AspNetCore.Annotations;
using Minimarket.Core.QueryFilters;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Filtros para la consulta de productos vendidos con paginación y criterios específicos.")]
    public class ProductSoldQueryPaginationResponse : PaginationQueryFilter
    {
        [SwaggerSchema("Id del producto (opcional).", Nullable = true)]
        public int? Id { get; set; }

        [SwaggerSchema("Nombre parcial o completo del producto para filtrar (opcional).", Nullable = true)]
        public string? Name { get; set; }

        [SwaggerSchema("Cantidad mínima vendida para filtrar (opcional).", Nullable = true)]
        public int? QuantitySold { get; set; }
    }
}
