using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.QueryFilters
{
    public class SaleQueryFilter : PaginationQueryFilter
    {
        public int ?Id { get; set; }
        public string ?CustomerName { get; set; }
    }
}
