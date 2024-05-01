using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace RSMFinalProjectAPI.Infrastructure.Database
{
    public class DatabaseInitializer
    {
        private readonly AdvWorksDbContext _context;

        public DatabaseInitializer(AdvWorksDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Ensure the database is created
                await _context.Database.EnsureCreatedAsync();

                // Check if the vSalesReport view exists
                var vSalesReportExists = await _context.Database.ExecuteSqlRawAsync(@"
                    SELECT COUNT(*) FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Sales' AND TABLE_NAME = 'vSalesReport'
                ") > 0;

                if (!vSalesReportExists)
                {
                    // Execute the SQL script to create the view
                    await _context.Database.ExecuteSqlRawAsync(@"
                        CREATE VIEW Sales.vSalesReport AS
                        SELECT 
                            soh.SalesOrderID AS OrderID, 
                            soh.OrderDate, 
                            soh.CustomerID, 
                            sod.ProductID, 
                            p.Name AS ProductName, 
                            pc.Name AS ProductCategory, 
                            sod.UnitPrice, 
                            sod.OrderQty AS Quantity, 
                            sod.LineTotal AS TotalPrice,
                            soh.SalesPersonID,
                            per.FirstName,
                            per.LastName,
                            pad.AddressLine1 AS ShippingAddress,
                            pad.AddressLine1 AS BillingAddress
                        FROM Sales.SalesOrderHeader soh
                        INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
                        INNER JOIN Production.Product p ON sod.ProductID = p.ProductID
                        INNER JOIN Production.ProductSubcategory psc ON p.ProductSubcategoryID = psc.ProductSubcategoryID
                        INNER JOIN Production.ProductCategory pc ON psc.ProductCategoryID = pc.ProductCategoryID
                        INNER JOIN Sales.Customer c ON soh.CustomerID = c.CustomerID
                        INNER JOIN Person.Person per ON c.PersonID = per.BusinessEntityID
                        INNER JOIN Person.Address pad ON soh.BillToAddressID = pad.AddressID
                    ");
                }

                // Check if the vSalesPerformance view exists
                var vSalesPerformanceExists = await _context.Database.ExecuteSqlRawAsync(@"
                    SELECT COUNT(*) FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'Sales' AND TABLE_NAME = 'vSalesPerformance'
                ") > 0;

                if (!vSalesPerformanceExists)
                {
                    // Execute the SQL script to create the vSalesPerformance view
                    await _context.Database.ExecuteSqlRawAsync(@"
                        CREATE VIEW Sales.vSalesPerformance AS
                        WITH SalesCTE AS (
                            SELECT
                                d.SalesOrderID as OrderID,
                                p.Name AS ProductName,
                                pc.Name AS ProductCategory,
                                SUM(s.TotalDue) AS TotalSales,
                                SUM(s.TotalDue) / SUM(SUM(s.TotalDue)) OVER (PARTITION BY t.Name) * 100 AS PercentageOfTotalSalesInRegion,
                                SUM(s.TotalDue) / SUM(SUM(s.TotalDue)) OVER (PARTITION BY t.Name, pc.Name) * 100 AS PercentageOfTotalCategorySalesInRegion
                            FROM
                                Sales.SalesOrderHeader s
                            JOIN
                                Sales.SalesOrderDetail d ON s.SalesOrderID = d.SalesOrderID
                            JOIN
                                Production.Product p ON d.ProductID = p.ProductID
                            JOIN
                                Production.ProductSubcategory psc ON p.ProductSubcategoryID = psc.ProductSubcategoryID
                            JOIN
                                Production.ProductCategory pc ON psc.ProductCategoryID = pc.ProductCategoryID
                            JOIN
                                Sales.SalesTerritory t ON s.TerritoryID = t.TerritoryID
                            WHERE
                                s.OrderDate <= GETDATE()
                            GROUP BY
                                d.SalesOrderID, p.Name, pc.Name, t.Name
                        )
                        SELECT
                            OrderID,
                            ProductName,
                            ProductCategory,
                            TotalSales,
                            PercentageOfTotalSalesInRegion,
                            PercentageOfTotalCategorySalesInRegion
                        FROM
                            SalesCTE
                    ");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during database initialization
                Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
            }
        }
    }
}
