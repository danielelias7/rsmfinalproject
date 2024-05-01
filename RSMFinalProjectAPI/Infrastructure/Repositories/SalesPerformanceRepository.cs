namespace RSMFinalProjectAPI.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using RSMFinalProjectAPI.Domain.Interfaces;
    using RSMFinalProjectAPI.Domain.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SalesPerformanceRepository : ISalesPerformanceRepository
    {
        private readonly AdvWorksDbContext _context;
        public SalesPerformanceRepository(AdvWorksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<vSalesPerformance>> GetAll()
        {
            return await _context.Set<vSalesPerformance>()
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
