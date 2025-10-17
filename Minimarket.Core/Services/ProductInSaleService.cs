using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Services
{
    public class ProductInSaleService : IProductInSaleService
    {
        private readonly IProductInSaleRepository _repo;
        private readonly ISaleRepository _saleRepo;
        private readonly IProductRepository _productRepo;
        public ProductInSaleService(IProductInSaleRepository repo, ISaleRepository saleRepo, IProductRepository productRepo)
        {
            _repo = repo;
            _saleRepo = saleRepo;
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<ProductInSale>> GetAllAsync(int idSale)
        {


            return await _repo.GetAllBySaleIdAsync(idSale);
        }
        public async Task<bool> CreateAsync(ProductInSale productInSale)
        {
            var validationResult = await _productRepo.GetByIdAsync(productInSale.IdProduct);
            if (validationResult == null)
            {
                throw new Exception("El producto en la venta no existe.");
            }

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
            var validationResult = await _productRepo.GetByIdAsync(productInSale.IdProduct);
            if (validationResult == null)
            {
                throw new Exception("El producto en la venta no existe.");
            }
            if (validationResult == null) {
                throw new Exception("El producto en la venta no existe.");
            }
            var existingProductInSale = await _repo.GetByIdAsync(productInSale.IdProduct);
            if (existingProductInSale == null)
            {
                throw new Exception("El producto en la venta no existe.");
            }
            await _repo.UpdateAsync(productInSale);
            return true;
        }
        public async Task<bool> DeleteAsync(int IdSale, int IdProduct)
        {
            var productInSale = await _repo.GetAllBySaleIdAsync(IdSale);
            if (productInSale == null)
            {
                throw new Exception("El IdSale no coincide con el producto en la venta.");
            }

            // Buscar por IdProduct en la lista de productos en la venta
            var product = productInSale.FirstOrDefault(p => p.IdProduct == IdProduct);
            if (product == null)
            {
                throw new Exception("El producto no se encuentra en la venta.");
            }

            await _repo.DeleteAsync(product);
            return true;
        }

    }
}
