using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Estructura estándar para respuestas de error desde la API.")]
    public class ErrorResponse
    {
        [SwaggerSchema("Tipo o categoría del error (ej. ValidationError, BussinesError).")]
        public string Type { get; set; }

        [SwaggerSchema("Mensaje de error legible para el consumidor de la API.")]
        public string Message { get; set; }

        [SwaggerSchema("Código de error interno o externo (opcional).")]
        public string ErrorCode { get; set; }

        [SwaggerSchema("Marca temporal (UTC) en que ocurrió el error.", Format = "date-time")]
        public DateTime Timestamp { get; set; }

        [SwaggerSchema("Ruta (path) de la petición que produjo el error.")]
        public string Path { get; set; }

        public ErrorResponse()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
