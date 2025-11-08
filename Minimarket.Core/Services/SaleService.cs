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
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> GetAllAsync(SaleQueryFilter filters)
        {
            var sales = await _unitOfWork.SaleRepository.GetAll();
            if(filters.Id != null)
            {
                sales = sales.Where(x => x.Id == filters.Id);
            }
            if(!string.IsNullOrEmpty(filters.CustomerName))
            {
                sales = sales.Where(x => x.CustomerName.ToLower().Contains(filters.CustomerName.ToLower()));
            }
            var pagedSales = PagedList<Object>.Create(sales, filters.PageNumber, filters.PageSize);

            if (pagedSales.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de ventas recuperados correctamente" } },
                    Pagination = pagedSales,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedSales,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
           }
        public async Task<Sale> GetByIdAsync(int id)
        {
            var sale = await _unitOfWork.SaleRepository.GetById(id);
            if(sale == null)
            {
                throw new BussinesException("La venta no existe.", (int)HttpStatusCode.NotFound);
            }
            return sale;
        }
        public async Task InsertAsync(Sale sale)
        {
            var customer = await _unitOfWork.UserRepository.GetById(sale.Id);
            if (customer == null)
            {
                throw new BussinesException("El cliente no existe.", (int)HttpStatusCode.NotFound);
            }
            
            await _unitOfWork.SaleRepository.Add(sale);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateAsync(Sale sale)
        {
            await _unitOfWork.SaleRepository.Update(sale);
            await _unitOfWork.SaveChangesAsync();
        }
        
    }
}
