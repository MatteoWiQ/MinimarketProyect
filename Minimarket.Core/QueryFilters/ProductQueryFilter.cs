using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Minimarket.Core.QueryFilters
{
    /// <summary>
    /// Filtra los parametros de consulta para productos (utilizado en endpoints que devuelven listas paginadas).
    /// </summary>
    [SwaggerSchema("Filtros para consultar productos con paginación y criterios opcionales.")]
    public class ProductQueryFilter : PaginationQueryFilter
    {
        [SwaggerSchema("Identificador del producto (opcional).", Nullable = true)]
        public int? Id { get; set; }

        [SwaggerSchema("Marca del producto para filtrar (opcional).")]
        public string ProductBrand { get; set; }

        [SwaggerSchema("Fecha de creación para filtrar registros a partir de esa fecha (opcional).", Nullable = true, Format = "date-time")]
        public DateTime? CreatedAt { get; set; }
    }
}
