namespace RSMFinalProjectAPI.Domain.Interfaces
{
    using RSMFinalProjectAPI.Domain.Models;

    public interface ISalesPerformanceRepository
    {
        Task<IEnumerable<vSalesPerformance>> GetAll();
    }
}