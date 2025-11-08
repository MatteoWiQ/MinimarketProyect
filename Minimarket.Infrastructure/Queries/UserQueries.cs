using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Queries
{
    public static class UserQueries
    {
        public static string GetAllUsers = @" select Id, UserType, FirstName, LastName, DateOfBirth, Telephone, IsActive, CreatedAt, Password " +
            "from dbo.[User];";
    }
}
