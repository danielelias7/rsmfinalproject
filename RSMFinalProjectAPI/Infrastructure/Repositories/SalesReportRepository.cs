namespace RSMFinalProjectAPI.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using RSMFinalProjectAPI.Domain.Interfaces;
    using RSMFinalProjectAPI.Domain.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SalesReportRepository : ISalesReportRepository
    {
        private readonly AdvWorksDbContext _context;
        public SalesReportRepository(AdvWorksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<vSalesReport>> GetAll()
        {
            return await _context.Set<vSalesReport>()
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
