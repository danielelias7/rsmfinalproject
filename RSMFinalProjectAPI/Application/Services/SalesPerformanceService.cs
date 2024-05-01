namespace RSMFinalProjectAPI.Application.Services
{
    using RSMFinalProjectAPI.Application.DTOs;
    using RSMFinalProjectAPI.Application.Exceptions;
    using RSMFinalProjectAPI.Domain.Interfaces;
    using RSMFinalProjectAPI.Domain.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SalesPerformanceService : ISalesPerformanceService
    {
        private readonly ISalesPerformanceRepository _salesPerformanceRepository;
        public SalesPerformanceService(ISalesPerformanceRepository repository)
        {
            _salesPerformanceRepository = repository;
        }
        
        public async Task<IEnumerable<GetSalesPerformanceDto>> GetAll(int pageNumber, int pageSize, int? orderId, string? productName, string? productCategory)
        {
            var salesPerformance = await _salesPerformanceRepository.GetAll();

            // Filters
            if (orderId.HasValue)
            {
                salesPerformance = salesPerformance.Where(s => s.OrderId == orderId.Value);
            }

            if (productName != null)
            {
                salesPerformance = salesPerformance.Where(s => s.ProductName.Contains(productName));
            }

            if (productCategory != null)
            {
                salesPerformance = salesPerformance.Where(s => s.ProductCategory == productCategory);
            }

            Console.WriteLine(orderId);

            // Pagination
            var paginatedSalesPerformance = salesPerformance
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            List<GetSalesPerformanceDto> salesPerformanceDto = new();

            foreach (var sales in paginatedSalesPerformance)
            {
                GetSalesPerformanceDto dto = new()
                {
                    OrderId = sales.OrderId,
                    ProductName = sales.ProductName,
                    ProductCategory = sales.ProductCategory,
                    TotalSales = sales.TotalSales,
                    PercentageOfTotalSalesInRegion = sales.PercentageOfTotalSalesInRegion,
                    PercentageOfTotalCategorySalesInRegion = sales.PercentageOfTotalSalesInRegion
                };

                salesPerformanceDto.Add(dto);
            }
            return salesPerformanceDto;
        }
    }
}
