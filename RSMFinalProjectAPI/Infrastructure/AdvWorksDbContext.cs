namespace RSMFinalProjectAPI.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    using RSMFinalProjectAPI.Domain.Models;

    using System.Reflection;

    public class AdvWorksDbContext : DbContext
    {
        public AdvWorksDbContext()
        {            
        }

        public AdvWorksDbContext(DbContextOptions<AdvWorksDbContext> options)
            : base(options) 
        {
        }

        public virtual DbSet<vSalesReport> SalesReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
    }
}