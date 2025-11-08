using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.QueryFilters;


namespace Minimarket.Core.Interface
{
    public interface IProductService
    {
        Task<ResponseData> GetAllAsync(ProductQueryFilter filter);
        Task<Product> GetByIdAsync(int id);
        Task InsertAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<ResponseData> GetProductsOrderByQuantitySoldAsync(ProductSoldQueryPaginationResponse filter);
        Task<ProductQueriesResponse> GetMostExpensiveProductAsync(ProductQueryFilter filter);
        Task<ResponseData> GetProductsThatNeverSoldAsync(ProductQueryFilter filter);
    }
}
