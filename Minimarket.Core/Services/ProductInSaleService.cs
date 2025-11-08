using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Services
{
    public class ProductInSaleService : IProductInSaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductInSaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllAsync(ProductInSaleQueryFilter filter)
        {
            var productsInSale = await _unitOfWork.ProductInSaleRepository.GetAllBySaleIdAsync(filter.IdSale);
            if (filter.IdProduct != null)
            {
                productsInSale = productsInSale.Where(x => x.IdProduct == filter.IdProduct);
            }
            if (filter.Quantity != null)
            {
                productsInSale = productsInSale.Where(x => x.Quantity == filter.Quantity);
            }

            var pagedProductsInSale = PagedList<Object>.Create(productsInSale, filter.PageNumber, filter.PageSize);
            if (pagedProductsInSale.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de productos en la venta recuperados correctamente" } },
                    Pagination = pagedProductsInSale,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedProductsInSale,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

        }
        public async Task<bool> CreateAsync(ProductInSale productInSale)
        {
            var validationResult = await _unitOfWork.ProductRepository.GetById(productInSale.IdProduct);
            if (validationResult == null)
            {
                throw new BussinesException("El producto en la venta no existe.", (int)HttpStatusCode.NotFound);
            }

            var sale = await _unitOfWork.SaleRepository.GetById(productInSale.IdSale);
            if (sale == null)
            {
                throw new BussinesException("La venta no existe.", (int)HttpStatusCode.NotFound);
            }
            await _unitOfWork.ProductInSaleRepository.AddAsync(productInSale);
            return true;
        }
        public async Task<bool> UpdateAsync(ProductInSale productInSale)
        {
            var validationResult = await _unitOfWork.ProductRepository.GetById(productInSale.IdProduct);
            if (validationResult == null)
            {
                throw new BussinesException("El producto en la venta no existe.", (int)HttpStatusCode.NotFound);
            }
            if (validationResult == null)
            {
                throw new BussinesException("El producto en la venta no existe.", (int)HttpStatusCode.NotFound);
            }
            var existingProductInSale = await _unitOfWork.ProductInSaleRepository.GetByIdAsync(productInSale.IdProduct);
            if (existingProductInSale == null)
            {
                throw new BussinesException("El producto en la venta no existe.", (int)HttpStatusCode.NotFound);
            }
            await _unitOfWork.ProductInSaleRepository.UpdateAsync(productInSale);
            return true;
        }
        public async Task<bool> DeleteAsync(int IdSale, int IdProduct)
        {
            var productInSale = await _unitOfWork.ProductInSaleRepository.GetAllBySaleIdAsync(IdSale);
            if (productInSale == null)
            {
                throw new BussinesException("El IdSale no coincide con el producto en la venta.", (int)HttpStatusCode.NotFound);
            }

            // Buscar por IdProduct en la lista de productos en la venta
            var product = productInSale.FirstOrDefault(p => p.IdProduct == IdProduct);
            if (product == null)
            {
                throw new BussinesException("El producto no se encuentra en la venta.", (int)HttpStatusCode.NotFound);
            }

            await _unitOfWork.ProductInSaleRepository.DeleteAsync(product);
            return true;
        }

        public async Task<ResponseData> GetDeailsProductInSale(ProductInSaleDetailsPagination filter)
        {
            var details = await _unitOfWork.ProductInSaleRepository.GetProductsInSaleDetailsBySaleIdAsync();
            if (filter.IdSale != null)
            {
                details = details.Where(x => x.IdSale == filter.IdSale);
            }

            if (!string.IsNullOrEmpty(filter.ProductName))
            {
                details = details.Where(x => x.ProductName!.Contains(filter.ProductName));
            }
            if (!string.IsNullOrEmpty(filter.ProductBrand))
            {
                details = details.Where(x => x.ProductBrand!.Contains(filter.ProductBrand));
            }
            if (filter.Quantity != null)
            {
                details = details.Where(x => x.Quantity == filter.Quantity);
            }
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                details = details.Where(x => x.CustomerName!.Contains(filter.CustomerName));
            }
            if (!string.IsNullOrEmpty(filter.PaymentMethod))
            {
                details = details.Where(x => x.PaymentMethod!.Contains(filter.PaymentMethod));
            }
            if (filter.IsDone != null)
            {
                details = details.Where(x => x.IsDone == filter.IsDone);
            }
            if (filter.Date != null)
            {
                details = details.Where(x => x.Date.Date == filter.Date.Value.Date);
            }

            var pagedDetails = PagedList<Object>.Create(details, filter.PageNumber, filter.PageSize);
            if (pagedDetails.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de detalles de productos en la venta recuperados correctamente" } },
                    Pagination = pagedDetails,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedDetails,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }
        }

        public Task<ResponseData> GetProductQuantityBySale(ProductQuantityBySalePagination filter)
        {
            var productQuantities = _unitOfWork.ProductInSaleRepository.GetProductQuantitiesBySaleIdAsync();
            if (filter.IdSale != null)
            {
                productQuantities = Task.FromResult(productQuantities.Result.Where(x => x.IdSale == filter.IdSale));
            }
            if (filter.TotalUnitsSold != null)
            {
                productQuantities = Task.FromResult(productQuantities.Result.Where(x => x.TotalUnitsSold == filter.TotalUnitsSold));
            }

            var pagedProductQuantities = PagedList<Object>.Create(productQuantities.Result, filter.PageNumber, filter.PageSize);
            if (pagedProductQuantities.Any())
            {
                return Task.FromResult(new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de cantidades de productos por venta recuperados correctamente" } },
                    Pagination = pagedProductQuantities,
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            }
            else
            {
                return Task.FromResult(new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedProductQuantities,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                });
            }
        }

        public async Task<ProductWithHighestQuantityInSaleResponse> GetProductWithHighestQuantityInSaleAsync()
        {
            return await _unitOfWork.ProductInSaleRepository.GetProductWithHighestQuantityInSaleAsync();
        }
    }

}