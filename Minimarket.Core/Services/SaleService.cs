using Minimarket.Core.Interfaces;
using Minimarket.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minimarket.Core.Interface;

namespace Minimarket.Core.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _unitOfWork.SaleRepository.GetAll();
        }
        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _unitOfWork.SaleRepository.GetById(id);
        }
        public async Task InsertAsync(Sale sale)
        {
            var customer = await _unitOfWork.UserRepository.GetById(sale.Id);
            if (customer == null)
            {
                throw new Exception("El cliente no existe.");
            }
            
            await _unitOfWork.SaleRepository.Add(sale);
        }
        public async Task UpdateAsync(Sale sale)
        {
            await _unitOfWork.SaleRepository.Update(sale);
        }
        
    }
}
