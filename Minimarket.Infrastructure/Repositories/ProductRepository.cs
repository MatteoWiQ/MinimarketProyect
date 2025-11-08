using Microsoft.EntityFrameworkCore;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;

using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Queries;
using Minimarket.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minimarket.Infraestructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        //private readonly MinimarketContext _context;
        private readonly IDapperContext _dapper;
        public ProductRepository(MinimarketContext ctx, IDapperContext dapper) : base(ctx)
        {
            //_context = ctx;
            _dapper = dapper;
        }

        public async Task<ProductQueriesResponse> GetMostExpensiveProductAsync()
        {
            try
            {
                var sql = ProductQueries.MostExpensiveProduct;
                return await _dapper.QueryFirstOrDefaultAsync<ProductQueriesResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<IEnumerable<GetProductsOrderByQuantitySoldResponse>> GetProductsOrderByQuantitySoldAsync()
        {
            try
            {
                var sql = ProductQueries.GetProductsOrderByQuantitySold;

                return await _dapper.QueryAsync<GetProductsOrderByQuantitySoldResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<IEnumerable<ProductQueriesResponse>> GetProductsThatNeverSold()
        {
            try
            {
                var sql = ProductQueries.ProductsThatNeverSold;
                return await _dapper.QueryAsync<ProductQueriesResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}