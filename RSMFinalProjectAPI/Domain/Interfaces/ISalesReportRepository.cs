namespace RSMFinalProjectAPI.Domain.Interfaces
{
    using RSMFinalProjectAPI.Domain.Models;

    public interface ISalesReportRepository
    {
        Task<IEnumerable<vSalesReport>> GetAll();
    }
}