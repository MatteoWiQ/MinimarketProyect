using Minimarket.Core.Interfaces;
using Minimarket.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repo;
        public SaleService(ISaleRepository repo)
        {
            _repo = repo;
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
            await _repo.AddAsync(sale);
        }
        public async Task UpdateAsync(Sale sale)
        {
            await _repo.UpdateAsync(sale);
        }
    }
}
