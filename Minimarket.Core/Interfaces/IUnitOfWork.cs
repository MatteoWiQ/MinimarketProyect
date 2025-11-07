using Minimarket.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<User> UserRepository { get; }
        IBaseRepository<Product> ProductRepository { get; }
        IBaseRepository<Sale> SaleRepository { get; }
        IProductInSaleRepository ProductInSaleRepository { get; }
        // Este repositorio no puede ser generico, tiene 2 IDs en su estructura
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
