using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Entidades;
using Minimarket.Infraestructure.Data;
using System.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minimarket.Core.Entidades;
using Minimarket.Core.Interface;

namespace Minimarket.Infraestructure.Repositories
{
    public class UserRepository : IUserRespository
    {
        private readonly MinimarketContext _context;
        public UserRepository(MinimarketContext ctx)
        {
            _context = ctx;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;

        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

       
    }
}