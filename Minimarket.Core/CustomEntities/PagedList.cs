using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    /// <summary>
    /// Lista paginada que contiene metadatos de paginación y la página actual de items.
    /// </summary>
    /// <typeparam name="T">Tipo de elementos en la página.</typeparam>
    public class PagedList<T> : List<T>
    {
        [SwaggerSchema("Página actual (1-based).")]
        public int CurrentPage { get; set; }

        [SwaggerSchema("Total de páginas.")]
        public int TotalPages { get; set; }

        [SwaggerSchema("Tamaño de página.")]
        public int PageSize { get; set; }

        [SwaggerSchema("Número total de elementos.")]
        public int TotalCount { get; set; }

        [SwaggerSchema("Indica si existe una página previa.", ReadOnly = true)]
        public bool HasPreviousPage => CurrentPage > 1;

        [SwaggerSchema("Indica si existe una página siguiente.", ReadOnly = true)]
        public bool HasNextPage => CurrentPage < TotalPages;

        [SwaggerSchema("Número de la siguiente página si existe (null si no).", ReadOnly = true)]
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;

        [SwaggerSchema("Número de la página previa si existe (null si no).", ReadOnly = true)]
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        /// <summary>
        /// Crea un PagedList a partir de una secuencia, con paginación por salto/limite.
        /// </summary>
        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
