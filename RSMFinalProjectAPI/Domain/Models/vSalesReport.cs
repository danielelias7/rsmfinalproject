namespace RSMFinalProjectAPI.Domain.Models
{
    public class vSalesReport
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCategory { get; set; }
        public decimal? UnitPrice { get; set; }
        public short Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? SalesPersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
    }
}