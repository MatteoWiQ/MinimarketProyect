using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Data;
using Minimarket.Core.Entities;
using Minimarket.Core.Interfaces;
using Minimarket.Infrastructure.Data;
using Minimarket.Infrastructure.Data.Context;
using Minimarket.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(MinimarketContext context, IDapperContext dapper) : base(context) { }

        public async Task<Security> GetLoginByCredentials(UserLogin login)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Login == login.User);
        }
    }
}
