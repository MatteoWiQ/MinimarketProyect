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
        private readonly IBaseRepository<Product> _repo;

        public ProductService(IBaseRepository<Product> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repo.GetAll();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task InsertAsync(Product product)
        {
            await _repo.Add(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _repo.Update(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.Delete(id);
        }
    }
}
