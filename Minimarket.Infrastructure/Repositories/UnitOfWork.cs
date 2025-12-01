using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Infraestructure.Repositories;
using Minimarket.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly MinimarketContext _context;
        private readonly IUserRepository? _userRepository;
        private readonly IProductRepository? _productRepository;
        private readonly ISaleRepository? _saleRepository;
        private readonly ProductInSaleRepository? _productSaleRepository;
        private readonly ISecurityRepository? _securityRepository;
        private IDbContextTransaction? _efTransaction;
        private IDapperContext _dapper;

        public UnitOfWork(MinimarketContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }


        public IProductRepository ProductRepository => 
            _productRepository ?? new ProductRepository(_context, _dapper);

        public ISaleRepository SaleRepository => 
            _saleRepository ?? new SaleRepository(_context, _dapper);

        public IProductInSaleRepository ProductInSaleRepository => 
            _productSaleRepository ?? new ProductInSaleRepository(_context, _dapper);

        public IUserRepository UserRepository => 
                _userRepository ?? new UserRepository(_context, _dapper);
        public ISecurityRepository SecurityRepository => 
                _securityRepository ?? new SecurityRepository(_context, _dapper);
        public void Dispose()
        {
            if (_context != null) {
                _efTransaction?.Dispose();
                _context.Dispose();
            }
        }
        public void SaveChanges() {     
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(); 
        }

        #region Transacciones
        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                // registrar la conexión/tx en DapperContext
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            // Retornamos la conexión subyacente del DbContext
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }
        #endregion

    }
}