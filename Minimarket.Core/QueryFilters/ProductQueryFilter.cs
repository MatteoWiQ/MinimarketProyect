using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.QueryFilters
{
    /// <summary>
    /// Filtra los parametros de post
    /// </summary>
    public class ProductQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// identificador del usuario que crea el post
        /// </summary>
        
        [SwaggerSchema("Identificador del usuario que crea el producto", Nullable = true)]
        public int ? Id { get; set; }
        public string ProductBrand { get; set; }
        public DateTime ? CreatedAt { get; set; }
    }
}
