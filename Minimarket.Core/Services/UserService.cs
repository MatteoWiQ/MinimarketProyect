using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Exceptions;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Core.QueryFilters;
using System.Net;
using System.Security.Cryptography;


namespace Minimarket.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllUsersAsync(UserQueryFilter filters)
        {

            var users = await _unitOfWork.UserRepository.GetAll();

            if (filters.Id != null)
            {
                users = users.Where(x => x.Id == filters.Id);
            }
            if (filters.CreatedAt != null)
            {
                users = users.Where(x => x.CreatedAt == filters.CreatedAt);
            }
            

            var pagedUsers = PagedList<Object>.Create(users, filters.PageNumber, filters.PageSize);

            if(pagedUsers.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de usuarios recuperados correctamente" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }



        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                throw new BussinesException("El usuario no existe.");
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
            // Verificar que el usuario exista antes de eliminar
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                
            }
            await _unitOfWork.UserRepository.Delete(id);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            return await _unitOfWork.UserRepository.getAllUsers();
        }
    }
}
