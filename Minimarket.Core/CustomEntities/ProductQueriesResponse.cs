using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Respuesta para consultas específicas sobre productos (ej. producto más caro, productos sin ventas, etc.).")]
    public class ProductQueriesResponse
    {
        [SwaggerSchema("Identificador del producto.")]
        public int Id { get; set; }

        [SwaggerSchema("Nombre del producto.")]
        public string Name { get; set; }

        [SwaggerSchema("Descripción del producto.")]
        public string Description { get; set; }

        [SwaggerSchema("Precio unitario del producto.", Format = "decimal")]
        public decimal Price { get; set; }

        [SwaggerSchema("Cantidad en stock.")]
        public int Stock { get; set; }

        [SwaggerSchema("Fecha de creación del producto (UTC).", Format = "date-time")]
        public DateTime CreatedAt { get; set; }
    }
}
