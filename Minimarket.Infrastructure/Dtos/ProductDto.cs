using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Minimarket.Infraestructure.Dtos
{
    [SwaggerSchema("Objeto DTO que representa un producto (entrada/salida de la API).")]
    public class ProductDto
    {
        [SwaggerSchema("Identificador único del producto.", ReadOnly = true)]
        public int Id { get; set; }

        [SwaggerSchema("Nombre del producto.")]
        public string Name { get; set; }

        [SwaggerSchema("Marca del producto (brand).")]
        public string ProductBrand { get; set; }

        [SwaggerSchema("Descripción del producto.")]
        public string Description { get; set; }

        [SwaggerSchema("Precio unitario del producto.", Format = "decimal")]
        public decimal Price { get; set; }

        [SwaggerSchema("Cantidad disponible en stock.")]
        public int Stock { get; set; }

        [SwaggerSchema("Indica si el producto está activo y puede venderse.")]
        public bool IsActive { get; set; }

        [SwaggerSchema("Fecha de creación del producto (UTC).", Format = "date-time", ReadOnly = true)]
        public DateTime CreatedAt { get; set; }
    }
}
