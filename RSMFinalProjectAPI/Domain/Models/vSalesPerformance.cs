namespace RSMFinalProjectAPI.Domain.Models
{
    public class vSalesPerformance
    {
        public int OrderId { get; set; }
        public string? ProductName { get; set;}
        public string? ProductCategory { get; set; }
        public decimal? TotalSales { get; set; }
        public decimal? PercentageOfTotalSalesInRegion { get; set; }
        public decimal? PercentageOfTotalCategorySalesInRegion { get; set; }

    }
}