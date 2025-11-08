using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.QueryFilters;
namespace Minimarket.Core.Interfaces
{
    public interface ISaleService
    {
        Task<ResponseData> GetAllAsync(SaleQueryFilter filters);
        Task<Sale> GetByIdAsync(int id);
        Task InsertAsync(Sale sale);
        Task UpdateAsync(Sale sale);
    }
}
