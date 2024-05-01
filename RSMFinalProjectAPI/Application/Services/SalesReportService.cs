namespace RSMFinalProjectAPI.Application.Services
{
    using RSMFinalProjectAPI.Application.DTOs;
    using RSMFinalProjectAPI.Application.Exceptions;
    using RSMFinalProjectAPI.Domain.Interfaces;
    using RSMFinalProjectAPI.Domain.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SalesReportService : ISalesReportService
    {
        private readonly ISalesReportRepository _salesReportRepository;
        public SalesReportService(ISalesReportRepository repository)
        {
            _salesReportRepository = repository;
        }
        
        public async Task<IEnumerable<GetSalesReportDto>> GetAll(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory)
        {
            var salesReport = await _salesReportRepository.GetAll();

            // Filters
            if (orderId.HasValue)
            {
                salesReport = salesReport.Where(s => s.OrderId == orderId.Value);
            }

            if (orderDate != null)
            {
                DateTime fechaFiltro = DateTime.Parse(orderDate);
                salesReport = salesReport.Where(s => s.OrderDate.Value.Date == fechaFiltro.Date);
            }

            if (productName != null)
            {
                salesReport = salesReport.Where(s => s.ProductName.Contains(productName));
            }

            if (productCategory != null)
            {
                salesReport = salesReport.Where(s => s.ProductCategory == productCategory);
            }

            Console.WriteLine(orderId);

            // Pagination
            var paginatedSalesReport = salesReport
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            List<GetSalesReportDto> salesReportDto = new();

            foreach (var sales in paginatedSalesReport)
            {
                GetSalesReportDto dto = new()
                {
                    OrderId = sales.OrderId,
                    OrderDate = sales.OrderDate,
                    CustomerId = sales.CustomerId,
                    ProductId = sales.ProductId,
                    ProductName = sales.ProductName,
                    ProductCategory = sales.ProductCategory,
                    UnitPrice = sales.UnitPrice,
                    Quantity = sales.Quantity,
                    TotalPrice = sales.TotalPrice,
                    SalesPersonId = sales.SalesPersonId,
                    FirstName = sales.FirstName,
                    LastName = sales.LastName,
                    ShippingAddress = sales.ShippingAddress,
                    BillingAddress = sales.BillingAddress
                };

                salesReportDto.Add(dto);
            }
            return salesReportDto;
        }
    }
}
