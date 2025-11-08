using Microsoft.EntityFrameworkCore;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interfaces;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Queries;
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
        private readonly IDapperContext _dapper;
        public ProductInSaleRepository(MinimarketContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }
        public async Task<IEnumerable<ProductInSale>> GetAllBySaleIdAsync(int IdSale)
        {

            var products = await _context.ProductInSales.Where(pis => pis.IdSale == IdSale).ToListAsync();
            return products;
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
        public async Task<IEnumerable<GetProductsInSaleDetailsResponse>> GetProductsInSaleDetailsBySaleIdAsync()
        {
            try
            {
                var sql = ProductInSaleQueries.GetProductsInSaleDetails;
                return await _dapper.QueryAsync<GetProductsInSaleDetailsResponse>(sql);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Task<IEnumerable<ProductQuantityBySaleResponse>> GetProductQuantitiesBySaleIdAsync()
        {
            try
            {
                var sql = ProductInSaleQueries.ProductQuantityBySale;
                return _dapper.QueryAsync<ProductQuantityBySaleResponse>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<ProductWithHighestQuantityInSaleResponse> GetProductWithHighestQuantityInSaleAsync()
        {
            try
            {
                var sql = ProductInSaleQueries.ProductWithHighestQuantityInSale;
                return _dapper.QueryFirstOrDefaultAsync<ProductWithHighestQuantityInSaleResponse>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
