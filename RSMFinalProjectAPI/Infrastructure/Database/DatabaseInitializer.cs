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

                // Check if the view exists
                var viewExists = await _context.Database.CanConnectAsync();

                if (!viewExists)
                {
                    // Execute the SQL script to create the view
                    await _context.Database.ExecuteSqlRawAsync(@"
                        IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vSalesReport' AND type = 'V')
                        BEGIN
                            EXEC('
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
                            ')
                        END
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
