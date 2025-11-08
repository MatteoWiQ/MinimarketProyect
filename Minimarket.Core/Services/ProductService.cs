using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllAsync(ProductQueryFilter filter)
        {

            var products = await _unitOfWork.ProductRepository.GetAll();

            if(filter.Id != null)
            {
                products = products.Where(x => x.Id == filter.Id);
            }
            if(!string.IsNullOrEmpty(filter.ProductBrand))
            {
                products = products.Where(x => x.ProductBrand.ToLower().Contains(filter.ProductBrand.ToLower()));
            }
            if(filter.CreatedAt != null)
            {
                products = products.Where(x => x.CreatedAt == filter.CreatedAt);
            }

            var pagedProducts = PagedList<Object>.Create(products, filter.PageNumber, filter.PageSize);
            if(pagedProducts.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de productos recuperados correctamente" } },
                    Pagination = pagedProducts,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedProducts,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(id);
            if(product == null)
            {
                throw new BussinesException("El producto no existe.", (int)HttpStatusCode.NotFound);
            }
            return product;
        }

        public async Task InsertAsync(Product product)
        {
            await _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product =  await _unitOfWork.ProductRepository.GetById(id);
            if(product == null)
            {
                throw new BussinesException("El producto no existe.", (int)HttpStatusCode.NotFound);
            }
            await _unitOfWork.ProductRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ResponseData> GetProductsOrderByQuantitySoldAsync(ProductSoldQueryPaginationResponse filter)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsOrderByQuantitySoldAsync();
            if (filter.Id != null)
            {
                products = products.Where(x => x.Id == filter.Id);
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                products = products.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            }
            
            var pagedProducts = PagedList<Object>.Create(products, filter.PageNumber, filter.PageSize);

            if(pagedProducts.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de productos recuperados correctamente" } },
                    Pagination = pagedProducts,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            return new ResponseData
            {
                Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                Pagination = pagedProducts,
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        public async Task<ProductQueriesResponse> GetMostExpensiveProductAsync(ProductQueryFilter filter)
        {
            var product =  _unitOfWork.ProductRepository.GetMostExpensiveProductAsync();
            return await product;
        }

        public Task<ResponseData> GetProductsThatNeverSoldAsync(ProductQueryFilter filter)
        {
            var products =  _unitOfWork.ProductRepository.GetProductsThatNeverSold();
            if(filter.Id != null)
            {
                products = Task.FromResult(products.Result.Where(x => x.Id == filter.Id));
            }
            
            if(filter.CreatedAt != null)
            {
                products = Task.FromResult(products.Result.Where(x => x.CreatedAt == filter.CreatedAt));
            }
            var pagedProducts = PagedList<Object>.Create(products.Result, filter.PageNumber, filter.PageSize);

            if (pagedProducts.Any())
            {
                return Task.FromResult(new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de productos recuperados correctamente" } },
                    Pagination = pagedProducts,
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            }
            return Task.FromResult(new ResponseData()
            {
                Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                Pagination = pagedProducts,
                StatusCode = System.Net.HttpStatusCode.NotFound
            });
        }
    }
}
