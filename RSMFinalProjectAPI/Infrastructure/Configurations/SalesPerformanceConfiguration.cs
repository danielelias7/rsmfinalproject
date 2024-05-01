namespace RSMFinalProjectAPI.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RSMFinalProjectAPI.Domain.Models;

    public class SalesPerformanceConfiguration : IEntityTypeConfiguration<vSalesPerformance>
    {
        public void Configure(EntityTypeBuilder<vSalesPerformance> builder)
        {
            builder.ToTable(nameof(vSalesPerformance), "Sales");

            builder.HasKey(e => e.OrderId);
            builder.Property(e => e.OrderId).HasColumnName("OrderID");
            builder.Property(e => e.ProductName).HasColumnName("ProductName");
            builder.Property(e => e.ProductCategory).HasColumnName("ProductCategory");
            builder.Property(e => e.TotalSales).HasColumnName("TotalSales");
            builder.Property(e => e.PercentageOfTotalSalesInRegion).HasColumnName("PercentageOfTotalSalesInRegion");
            builder.Property(e => e.PercentageOfTotalCategorySalesInRegion).HasColumnName("PercentageOfTotalCategorySalesInRegion");
        }
    }
}