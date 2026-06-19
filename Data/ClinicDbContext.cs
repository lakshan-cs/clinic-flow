using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Patient> Patients { get; set; }
   
    }
}
