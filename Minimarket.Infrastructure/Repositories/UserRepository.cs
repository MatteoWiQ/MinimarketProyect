using Microsoft.EntityFrameworkCore;
using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Dtos;
using Minimarket.Core.Interface;
using Minimarket.Core.Interfaces;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Queries;
using Minimarket.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minimarket.Infraestructure.Repositories
{
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        //private readonly MinimarketContext _context;
        private readonly IDapperContext _dapper;
        public UserRepository(MinimarketContext ctx, IDapperContext dapper) : base(ctx)
        {
            //_context = ctx;
            _dapper = dapper;
        }

        public async Task<IEnumerable<UserResponse>> getAllUsers()
        {
            try
            {
                var sql = UserQueries.GetAllUsers;
                return await _dapper.QueryAsync<UserResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}