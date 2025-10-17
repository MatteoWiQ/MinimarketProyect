using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Minimarket.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task InsertAsync(User user)
        {
            await _repo.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _repo.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            await _repo.DeleteAsync(user);
            return true;

        }

    }
}

