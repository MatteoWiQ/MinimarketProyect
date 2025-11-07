using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Infraestructure.Repositories;
using Minimarket.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly MinimarketContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<Sale> _saleRepository;
        private readonly ProductInSaleRepository _productSaleRepository;
        public UnitOfWork(MinimarketContext context)
        {
            _context = context;
        }
        public IBaseRepository<User> UserRepository =>
            _userRepository ?? new UserRepository(_context);

        public IBaseRepository<Product> ProductRepository => 
            _productRepository ?? new ProductRepository(_context);

        public IBaseRepository<Sale> SaleRepository => 
            _saleRepository ?? new SaleRepository(_context);

        public IProductInSaleRepository ProductInSaleRepository => 
            _productSaleRepository ?? new ProductInSaleRepository(_context);

        public void Dispose()
        {
            if(_context != null) _context.Dispose();
        }
        public void SaveChanges() {     
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(); 
        }
    }
}