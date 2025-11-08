using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Data.Entities;

using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
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
        //private readonly MinimarketContext _context;
        private readonly IDapperContext _dapper;
        public ProductRepository(MinimarketContext ctx, IDapperContext dapper) : base(ctx)
        {
            //_context = ctx;
            _dapper = dapper;
        }

    }
}