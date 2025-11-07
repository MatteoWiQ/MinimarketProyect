using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Data.Entities;

using Minimarket.Core.Interface;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minimarket.Infraestructure.Repositories
{
    public class ProductRepository : BaseRepository<Product> ,IProductRepository 
    {
        private readonly MinimarketContext _context;
        public ProductRepository(MinimarketContext ctx) : base(ctx)
        {
            _context = ctx;
        }

    }
}