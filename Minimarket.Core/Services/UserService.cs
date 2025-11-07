using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;


namespace Minimarket.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _repo;

        public UserService( IBaseRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repo.GetAll();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task InsertAsync(User user)
        {
            await _repo.Add(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _repo.Update(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.Delete(id);
        }

    }
}

