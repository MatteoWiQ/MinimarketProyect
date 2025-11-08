using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    public class ProductQuantityBySaleResponse
    {
        public int IdSale { get; set; }
        public int DistinctProductsCount { get; set; }
        public int TotalUnitsSold { get; set; }
        public decimal SaleTotalAmount { get; set; }
    }
}
