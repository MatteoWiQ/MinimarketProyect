using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    [SwaggerSchema("Metadatos de paginación devueltos junto con listas paginadas.")]
    public class Pagination
    {
        [SwaggerSchema("Número total de elementos disponibles.")]
        public int TotalCount { get; set; }

        [SwaggerSchema("Tamaño de página utilizado.")]
        public int PageSize { get; set; }

        [SwaggerSchema("Página actual (1-based).")]
        public int CurrentPage { get; set; }

        [SwaggerSchema("Número total de páginas.")]
        public int TotalPages { get; set; }

        [SwaggerSchema("Indica si hay una página siguiente.")]
        public bool HasNextPage { get; set; }

        [SwaggerSchema("Indica si hay una página previa.")]
        public bool HasPreviousPage { get; set; }

        public Pagination()
        {

        }

        
        public Pagination(PagedList<object> lista)
        {
            TotalCount = lista.TotalCount;
            PageSize = lista.PageSize;
            CurrentPage = lista.CurrentPage;
            TotalPages = lista.TotalPages;
            HasNextPage = lista.HasNextPage;
            HasPreviousPage = lista.HasPreviousPage;
        }
    }
}
