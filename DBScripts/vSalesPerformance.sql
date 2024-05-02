CREATE VIEW Sales.vSalesPerformance AS
WITH
    SalesCTE AS (
        SELECT
            d.SalesOrderID as OrderID, p.Name AS ProductName, pc.Name AS ProductCategory, SUM(s.TotalDue) AS TotalSales, SUM(s.TotalDue) / SUM(SUM(s.TotalDue)) OVER (
                PARTITION BY
                    t.Name
            ) * 100 AS PercentageOfTotalSalesInRegion, SUM(s.TotalDue) / SUM(SUM(s.TotalDue)) OVER (
                PARTITION BY
                    t.Name, pc.Name
            ) * 100 AS PercentageOfTotalCategorySalesInRegion
        FROM
            Sales.SalesOrderHeader s
            JOIN Sales.SalesOrderDetail d ON s.SalesOrderID = d.SalesOrderID
            JOIN Production.Product p ON d.ProductID = p.ProductID
            JOIN Production.ProductSubcategory psc ON p.ProductSubcategoryID = psc.ProductSubcategoryID
            JOIN Production.ProductCategory pc ON psc.ProductCategoryID = pc.ProductCategoryID
            JOIN Sales.SalesTerritory t ON s.TerritoryID = t.TerritoryID
        WHERE
            s.OrderDate <= GETDATE ()
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
FROM SalesCTE