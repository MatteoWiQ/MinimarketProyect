using Minimarket.Core.Data.Entities;
namespace Minimarket.Core.Interfaces
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<Sale> GetByIdAsync(int id);
        Task InsertAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        
    }
}
