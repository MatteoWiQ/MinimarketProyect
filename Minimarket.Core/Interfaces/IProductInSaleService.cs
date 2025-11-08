using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.QueryFilters;

namespace Minimarket.Core.Interfaces
{
    public interface IProductInSaleService
    {
        Task<ResponseData> GetAllAsync(ProductInSaleQueryFilter filter);
        Task<bool> CreateAsync(ProductInSale productInSale);
        Task<bool> UpdateAsync(ProductInSale productInSale);
        Task<bool> DeleteAsync(int IdSale, int IdProduct);
        Task<ResponseData> GetDeailsProductInSale(ProductInSaleDetailsPagination filter);
        Task<ResponseData> GetProductQuantityBySale(ProductQuantityBySalePagination filter);
        Task<ProductWithHighestQuantityInSaleResponse> GetProductWithHighestQuantityInSaleAsync();
    }
}
