namespace RSMFinalProjectAPI.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RSMFinalProjectAPI.Domain.Models;

    public class SalesReportConfiguration : IEntityTypeConfiguration<vSalesReport>
    {
        public void Configure(EntityTypeBuilder<vSalesReport> builder)
        {
            builder.ToTable(nameof(vSalesReport), "Sales");

            builder.HasKey(e => e.OrderId);
            builder.Property(e => e.OrderId).HasColumnName("OrderID");
            builder.Property(e => e.OrderDate).HasColumnName("OrderDate");
            builder.Property(e => e.CustomerId).HasColumnName("CustomerId");
            builder.Property(e => e.ProductId).HasColumnName("ProductId");
            builder.Property(e => e.ProductName).HasColumnName("ProductName");
            builder.Property(e => e.ProductCategory).HasColumnName("ProductCategory");
            builder.Property(e => e.UnitPrice).HasColumnName("UnitPrice");
            builder.Property(e => e.Quantity).HasColumnName("Quantity");
            builder.Property(e => e.TotalPrice).HasColumnName("TotalPrice");
            builder.Property(e => e.SalesPersonId).HasColumnName("SalesPersonId");
            builder.Property(e => e.FirstName).HasColumnName("FirstName");
            builder.Property(e => e.LastName).HasColumnName("LastName");
            builder.Property(e => e.ShippingAddress).HasColumnName("ShippingAddress");
            builder.Property(e => e.BillingAddress).HasColumnName("BillingAddress");
        }
    }
}