using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Queries
{
    public static class ProductInSaleQueries
    {
        //Detalle de ProductInSale con la información del producto de cada venta
        public static string GetProductsInSaleDetails = @"
                                SELECT
                                    pis.IdSale,
                                    pis.IdProduct,
                                    p.[Name]       AS ProductName,
                                    p.ProductBrand,
                                    pis.Quantity,
                                    pis.UnitPrice,
                                    (pis.Quantity * pis.UnitPrice) AS LineTotal,
                                    s.CustomerId,
                                    s.CustomerName,
                                    s.PaymentMethod,
                                    s.IsDone,
                                    s.[Date]
                                FROM dbo.ProductInSale pis
                                JOIN dbo.Product p
                                    ON p.Id = pis.IdProduct
                                LEFT JOIN dbo.Sale s
                                    ON s.Id = pis.IdSale
                                ORDER BY pis.IdSale, pis.IdProduct;   
                            ";
        //Cantidad de productos vendidos por venta, total de unidades vendidas y monto total de la venta
        public static string ProductQuantityBySale = @"
                            SELECT
                                pis.IdSale,
                                COUNT(*) AS DistinctProductsCount,   
                                SUM(pis.Quantity) AS TotalUnitsSold,
                                SUM(pis.Quantity * pis.UnitPrice) AS SaleTotalAmount
                            FROM dbo.ProductInSale pis
                            GROUP BY pis.IdSale
                            ORDER BY TotalUnitsSold DESC; 
                            ";
        // venta con mayor cantidad de un solo producto
        public static string ProductWithHighestQuantityInSale = @"
                            SELECT TOP (1)
                                pis.IdSale,
                                pis.IdProduct,
                                p.[Name] AS ProductName,
                                pis.Quantity,
                                pis.UnitPrice,
                                (pis.Quantity * pis.UnitPrice) AS LineTotal,
                                s.CustomerId,
                                s.CustomerName,
                                s.[Date]
                            FROM dbo.ProductInSale pis
                            JOIN dbo.Product p ON p.Id = pis.IdProduct
                            LEFT JOIN dbo.Sale s ON s.Id = pis.IdSale
                            ORDER BY pis.Quantity DESC;
                            ";
    }
}
