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
        private readonly ISaleRepository _repo;
        private readonly IUserRepository _customerRepo;
        public SaleService(ISaleRepository repo , IUserRepository customerRepo)
        {
            _repo = repo;
            _customerRepo = customerRepo;
        }
        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task InsertAsync(Sale sale)
        {
            // Verificar si el id del customer existe
            var customer = await _customerRepo.GetByIdAsync((int)sale.CustomerId);
            if (customer == null)
            {
                throw new Exception("El cliente no existe.");
            }
            
            await _repo.AddAsync(sale);
        }
        public async Task UpdateAsync(Sale sale)
        {
            await _repo.UpdateAsync(sale);
        }
        
    }
}
