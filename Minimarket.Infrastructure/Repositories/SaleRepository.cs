using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interfaces;
using Minimarket.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly MinimarketContext _context;
        public SaleRepository(MinimarketContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            var sales = await _context.Sales.ToListAsync();
            return sales;
        }
        public async Task<Sale> GetByIdAsync(int id)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);
            return sale;
        }
        public async Task AddAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Sale sale)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }

}
