using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Queries
{
    /// <summary>
    /// Consultas SQL crudas usadas por los repositorios / servicios para reports rápidos.
    /// </summary>
    public static class ProductQueries
    {
        /// <summary>
        /// La cantidad de productos vendidos ordenados por mayor cantidad vendida.
        /// </summary>
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

        /// <summary>
        /// Producto más caro del supermercado.
        /// </summary>
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

        /// <summary>
        /// Productos que nunca se vendieron.
        /// </summary>
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
