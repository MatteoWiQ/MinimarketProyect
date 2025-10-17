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
    public class ProductInSaleRepository : IProductInSaleRepository
    {
        private readonly MinimarketContext _context;
        public ProductInSaleRepository(MinimarketContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductInSale>> GetAllBySaleIdAsync(int IdSale)
        {
            var productsInSale = await _context.ProductInSales
                .Where(pis => pis.IdSale == IdSale)
                .ToListAsync();
            return productsInSale;
        }
        
        public async Task DeleteAsync(ProductInSale productInSale)
        {
            _context.ProductInSales.Remove(productInSale);
            await _context.SaveChangesAsync();
        }
        public async Task<ProductInSale> GetByIdAsync(int id)
        {
            var productInSale = await _context.ProductInSales.FirstOrDefaultAsync(pis => pis.IdSale == id);
            return productInSale;
        }
        public async Task AddAsync(ProductInSale productInSale)
        {
            _context.ProductInSales.Add(productInSale);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ProductInSale productInSale)
        {
            _context.ProductInSales.Update(productInSale);
            await _context.SaveChangesAsync();
        }
    }
}
