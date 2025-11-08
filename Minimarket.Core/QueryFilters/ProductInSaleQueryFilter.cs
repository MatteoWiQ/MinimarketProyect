using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.QueryFilters
{
    public class ProductInSaleQueryFilter : PaginationQueryFilter
    {
        public int? IdProduct { get; set; }
        public int IdSale { get; set; }
        public int? Quantity { get; set; }
    }
}
