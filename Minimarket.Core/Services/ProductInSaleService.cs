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
        private readonly IUnitOfWork _unitOfWork;
        public ProductInSaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductInSale>> GetAllAsync(int idSale)
        {
            return await _unitOfWork.ProductInSaleRepository.GetAllBySaleIdAsync(idSale);
        }
        public async Task<bool> CreateAsync(ProductInSale productInSale)
        {
            var validationResult = await _unitOfWork.ProductRepository.GetById(productInSale.IdProduct);
            if (validationResult == null)
            {
                throw new Exception("El producto en la venta no existe.");
            }

            var sale = await _unitOfWork.SaleRepository.GetById(productInSale.IdSale);
            if (sale == null)
            {
                throw new Exception("La venta no existe.");
            }
            await _unitOfWork.ProductInSaleRepository.AddAsync(productInSale);
            return true;
        }
        public async Task<bool> UpdateAsync(ProductInSale productInSale)
        {
            var validationResult = await _unitOfWork.ProductRepository.GetById(productInSale.IdProduct);
            if (validationResult == null)
            {
                throw new Exception("El producto en la venta no existe.");
            }
            if (validationResult == null) {
                throw new Exception("El producto en la venta no existe.");
            }
            var existingProductInSale = await _unitOfWork.ProductInSaleRepository.GetByIdAsync(productInSale.IdProduct);
            if (existingProductInSale == null)
            {
                throw new Exception("El producto en la venta no existe.");
            }
            await _unitOfWork.ProductInSaleRepository.UpdateAsync(productInSale);
            return true;
        }
        public async Task<bool> DeleteAsync(int IdSale, int IdProduct)
        {
            var productInSale = await _unitOfWork.ProductInSaleRepository.GetAllBySaleIdAsync(IdSale);
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

            await _unitOfWork.ProductInSaleRepository.DeleteAsync(product);
            return true;
        }

    }
}
