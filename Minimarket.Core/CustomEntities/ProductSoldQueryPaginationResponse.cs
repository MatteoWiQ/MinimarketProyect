using Minimarket.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    public class ProductSoldQueryPaginationResponse : PaginationQueryFilter
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? QuantitySold { get; set; }
    }
}
