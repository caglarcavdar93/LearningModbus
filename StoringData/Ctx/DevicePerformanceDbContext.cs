using Microsoft.EntityFrameworkCore;

namespace StoringData.Ctx
{
    public class DevicePerformanceDbContext : DbContext
    {
        public DevicePerformanceDbContext(DbContextOptions<DevicePerformanceDbContext> options) : base(options) 
        {

        }
        public virtual DbSet<DevicePerformance> DevicePerformances { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DevicePerformance>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
