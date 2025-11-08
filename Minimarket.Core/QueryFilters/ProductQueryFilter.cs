using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.QueryFilters
{
    public class ProductQueryFilter : PaginationQueryFilter
    {
        public int ? Id { get; set; }
        public string ProductBrand { get; set; }
        public DateTime ? CreatedAt { get; set; }
    }
}
