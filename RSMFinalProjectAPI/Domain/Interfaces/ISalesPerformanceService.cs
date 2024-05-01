namespace RSMFinalProjectAPI.Domain.Interfaces
{
    using RSMFinalProjectAPI.Application.DTOs;

    public interface ISalesPerformanceService
    {
        Task<IEnumerable<GetSalesPerformanceDto>> GetAll(int pageNumber, int pageSize, int? orderId, string? productName, string? productCategory);
    }
}
