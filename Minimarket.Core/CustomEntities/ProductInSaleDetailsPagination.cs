using Minimarket.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    public class ProductInSaleDetailsPagination : PaginationQueryFilter
    {
        public int? IdSale { get; set; }
        public int? IdProduct { get; set; }
        public string? ProductName { get; set; }
        public string? ProductBrand { get; set; }
        public int? Quantity { get; set; }
        public string? CustomerName { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? IsDone { get; set; }
        public DateTime? Date { get; set; }
    }
}
