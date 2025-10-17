using Minimarket.Core.Data.Entities;

namespace Minimarket.Core.Interfaces
{
    public interface IProductInSaleService
    {
        Task<IEnumerable<ProductInSale>> GetAllAsync(int idSale);
        Task<bool> CreateAsync(ProductInSale productInSale);
        Task<bool> UpdateAsync(ProductInSale productInSale);
        Task<bool> DeleteAsync(int IdSale);

    }
}
