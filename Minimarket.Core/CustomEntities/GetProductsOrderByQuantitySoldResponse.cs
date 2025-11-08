using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    public class GetProductsOrderByQuantitySoldResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductBrand { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
