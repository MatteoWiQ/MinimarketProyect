using Minimarket.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Interfaces
{
    public interface IProductInSaleRepository
    {
        Task<IEnumerable<ProductInSale>> GetAllBySaleIdAsync(int saleId);
        Task<ProductInSale> GetByIdAsync(int id);
        Task AddAsync(ProductInSale productInSale);
        Task UpdateAsync(ProductInSale productInSale);
        Task DeleteAsync(ProductInSale productInSale);
    }
}
