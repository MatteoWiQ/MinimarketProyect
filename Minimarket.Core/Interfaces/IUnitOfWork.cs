using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        ISaleRepository SaleRepository { get; }
        ISecurityRepository SecurityRepository { get; }
        IProductInSaleRepository ProductInSaleRepository { get; }
        // Este repositorio no puede ser generico, tiene 2 IDs en su estructura
        void SaveChanges();
        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();

    }
}
