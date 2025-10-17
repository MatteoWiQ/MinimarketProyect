using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Services
{
    public class ProductInSaleService : IProductInSaleService
    {
        private readonly IProductInSaleRepository _repo;
        private readonly ISaleRepository _saleRepo;
        ProductInSaleService(IProductInSaleRepository repo, ISaleRepository saleRepo)
        {
            _repo = repo;
            _saleRepo = saleRepo;
        }

        public async Task<IEnumerable<ProductInSale>> GetAllAsync(int idSale)
        {

            var productsInSale = await _repo.GetAllBySaleIdAsync(idSale);
            return productsInSale;
        }
        public async Task<bool> CreateAsync(ProductInSale productInSale)
        {
            // Verificar si el id de la venta existe
            var sale = await _saleRepo.GetByIdAsync(productInSale.IdSale);
            if (sale == null)
            {
                throw new Exception("La venta no existe.");
            }
            await _repo.AddAsync(productInSale);
            return true;
        }
        public async Task<bool> UpdateAsync(ProductInSale productInSale)
        {
            var existingProductInSale = await _repo.GetByIdAsync(productInSale.IdProduct);
            if (existingProductInSale == null)
            {
                return false;
            }
            await _repo.UpdateAsync(productInSale);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var productInSale = await _repo.GetByIdAsync(id);
            if (productInSale == null)
            {
                return false;
            }
            await _repo.DeleteAsync(productInSale);
            return true;
        }

    }
}
