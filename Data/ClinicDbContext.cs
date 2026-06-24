using Microsoft.EntityFrameworkCore;
using ClinicFlow.Models;

namespace ClinicFlow.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Clinic> Clinics { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Allergy> Allergies { get; set; }

        public DbSet<PatientAllergy> PatientAllergies { get; set; }

    }
}
