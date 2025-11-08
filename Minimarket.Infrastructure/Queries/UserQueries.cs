using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Queries
{
    public static class UserQueries
    {
        //Usuario que hizo más ventas
        public static string UserWithMostSales = @"
                            SELECT TOP (1)
                                u.Id,
                                u.FirstName,
                                u.LastName,
                                u.Email,
                                COUNT(s.Id) AS TotalSales
                            FROM dbo.[User] u
                            JOIN dbo.Sale s ON TRY_CAST(s.CustomerId AS INT) = u.Id
                            GROUP BY u.Id, u.FirstName, u.LastName, u.Email
                            ORDER BY TotalSales DESC;
                            ";
        // Edad de los usuarios ordenados de mayor a menor
        public static string AgeOfUsers = @"
                            SELECT 
                                u.Id,
                                u.UserType,
                                u.FirstName,
                                u.LastName,
                                u.DateOfBirth,
                                DATEDIFF(YEAR, u.DateOfBirth, GETDATE()) 
                                    - CASE 
                                        WHEN MONTH(GETDATE()) < MONTH(u.DateOfBirth)
                                             OR (MONTH(GETDATE()) = MONTH(u.DateOfBirth) AND DAY(GETDATE()) < DAY(u.DateOfBirth))
                                          THEN 1 
                                          ELSE 0 
                                      END AS Age
                            FROM dbo.[User] AS u
                            WHERE u.DateOfBirth IS NOT NULL
                            ORDER BY Age DESC;
                            ";
        // Cantidad de usuarios por UserType
        public static string SummarizeUsersByRole = @"
                            SELECT UserType,
                            COUNT(*) AS UsersCount
                            FROM dbo.[User]
                            GROUP BY UserType
                            ORDER BY UsersCount DESC;
                            ";
    }
}
