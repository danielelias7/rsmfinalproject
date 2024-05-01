namespace RSMFinalProjectAPI.Domain.Interfaces
{
    using RSMFinalProjectAPI.Application.DTOs;

    public interface ISalesReportService
    {
        Task<IEnumerable<GetSalesReportDto>> GetAll(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory);

        Task CreatePdfReport(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory);
    }
}
