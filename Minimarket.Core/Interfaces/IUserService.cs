using Minimarket.Core.CustomEntities;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.QueryFilters;


namespace Minimarket.Core.Interface
{
    public interface IUserService
    {
        Task<ResponseData> GetAllUsersAsync(UserQueryFilter filters);
        Task<User> GetByIdAsync(int id);
        Task InsertAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<UserWithMostSalesResponse> GetUserWithMostSales();
        Task<ResponseData> GetAllAgeUsers(AgeUsersPaginationResponse filters);
        Task<ResponseData> GetSummarizeTypeOfUsers(SummarizeUserTypePagination filters);
    }
}
