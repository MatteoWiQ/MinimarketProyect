using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _unitOfWork.ProductRepository.GetAll();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _unitOfWork.ProductRepository.GetById(id);
        }

        public async Task InsertAsync(Product product)
        {
            await _unitOfWork.ProductRepository.Add(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _unitOfWork.ProductRepository.Update(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.ProductRepository.Delete(id);
        }
    }
}
