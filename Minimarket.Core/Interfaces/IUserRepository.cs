using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        //Task<IEnumerable<User>> GetAllAsync();
        //Task<User> GetByIdAsync(int id);
        //Task AddAsync(User user);
        //Task UpdateAsync(User user);
        //Task DeleteAsync(User user);
        Task<UserWithMostSalesResponse> GetUserWithMostSales();
        Task<IEnumerable<AgeOfUsersResponse>> GetAllAgeUsers();
        Task<IEnumerable<SummarizeTypeOfUsers>> GetSummarizeTypeOfUsers();
    }
}
