using Minimarket.Core.CustomEntities;
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
        // Retornar lista de productos en una venta
        Task<ProductInSale> GetByIdAsync(int id);
        Task AddAsync(ProductInSale productInSale);
        Task UpdateAsync(ProductInSale productInSale);
        Task DeleteAsync(ProductInSale productInSale);
        Task<IEnumerable<GetProductsInSaleDetailsResponse>> GetProductsInSaleDetailsBySaleIdAsync();
        Task<IEnumerable<ProductQuantityBySaleResponse>> GetProductQuantitiesBySaleIdAsync();
        Task<ProductWithHighestQuantityInSaleResponse> GetProductWithHighestQuantityInSaleAsync();
    }
}
