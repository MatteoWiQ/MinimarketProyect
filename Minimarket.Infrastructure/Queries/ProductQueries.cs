        using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Queries
{
    public static class ProductQueries
    {
        // La cantidad de productos vendidos ordenados por mayor cantidad vendida
        public static readonly string GetProductsOrderByQuantitySold = @"
                    SELECT 
                        p.Id, 
                        p.[Name], 
                        p.ProductBrand, 
                        SUM(ISNULL(pis.Quantity, 0)) AS TotalQuantitySold
                    FROM dbo.Product p
                    LEFT JOIN dbo.ProductInSale pis ON pis.IdProduct = p.Id
                    GROUP BY p.Id, p.[Name], p.ProductBrand
                    ORDER BY TotalQuantitySold DESC;
            ";
        // Producto más caro del supermercado
        public static string MostExpensiveProduct = @"
                            SELECT TOP (1)
                                p.Id,
                                p.[Name],
                                p.Description,
                                p.Price,
                                p.Stock,
                                p.CreatedAt
                            FROM dbo.Product p
                            ORDER BY p.Price DESC
                            ";
        // Productos que nunca se vendieron
        public static string ProductsThatNeverSold = @"
                            SELECT
                                p.Id,
                                p.[Name],
                                p.Description,
                                p.Price,
                                p.Stock,
                                p.IsActive,
                                p.CreatedAt
                            FROM dbo.Product p
                            LEFT JOIN dbo.ProductInSale pis ON p.Id = pis.IdProduct
                            WHERE pis.IdProduct IS NULL
                            ";
    }
}
