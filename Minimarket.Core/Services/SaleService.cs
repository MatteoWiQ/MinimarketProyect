using Minimarket.Core.Interfaces;
using Minimarket.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minimarket.Core.Interface;

namespace Minimarket.Core.Services
{
    public class SaleService : ISaleService
    {
        private readonly IBaseRepository<Sale> _repo;
        private readonly IBaseRepository<User> _customerRepo;
        public SaleService(IBaseRepository<Sale> repo , IBaseRepository<User> customerRepo)
        {
            _repo = repo;
            _customerRepo = customerRepo;
        }
        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _repo.GetAll();
        }
        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _repo.GetById(id);
        }
        public async Task InsertAsync(Sale sale)
        {
            var customer = await _customerRepo.GetById(sale.Id);
            if (customer == null)
            {
                throw new Exception("El cliente no existe.");
            }
            
            await _repo.Add(sale);
        }
        public async Task UpdateAsync(Sale sale)
        {
            await _repo.Update(sale);
        }
        
    }
}
