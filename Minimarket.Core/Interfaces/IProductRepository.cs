using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<GetProductsOrderByQuantitySoldResponse>> GetProductsOrderByQuantitySoldAsync();    
        Task<ProductQueriesResponse> GetMostExpensiveProductAsync();
        Task<IEnumerable<ProductQueriesResponse>> GetProductsThatNeverSold();
    }
}
