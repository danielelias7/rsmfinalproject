namespace RSMFinalProjectAPI.Domain.Interfaces
{
    using RSMFinalProjectAPI.Application.DTOs;
    using System.IO;
    using System.Threading.Tasks;

    public interface ISalesReportService
    {
        Task<IEnumerable<GetSalesReportDto>> GetAll(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory);
        Task<Stream> CreatePdfReport(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory);
    }
}
