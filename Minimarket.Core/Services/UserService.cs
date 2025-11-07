using Minimarket.Core.Data.Entities;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;


namespace Minimarket.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _unitOfWork.UserRepository.GetAll();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                throw new BussinesException("El usuario no existe");
            }
            return user;
        }

        public async Task InsertAsync(User user)
        {
            await _unitOfWork.UserRepository.Add(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _unitOfWork.UserRepository.Update(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.UserRepository.Delete(id);
        }

    }
}

