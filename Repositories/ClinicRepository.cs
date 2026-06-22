using ClinicFlow.Data;
using ClinicFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Repositories
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly ClinicDbContext context;

        public ClinicRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Clinic> GetClinics()
        {
            return context.Clinics.ToList();
        }

        public Clinic GetClinicById(int id)
        {
            return context.Clinics.Find(id);
        }

        public void AddClinic(Clinic clinic)
        {
            context.Clinics.Add(clinic);
            context.SaveChanges();
        }

        public void UpdateClinic(Clinic clinic)
        {
            context.Clinics.Find(clinic.Id);
            context.SaveChanges();
        }

        public void DeleteClinic(int clinicID)
        {
            Clinic clinic = context.Clinics.Find(clinicID);
            if (clinic == null) return;
            context.Clinics.Remove(clinic);
            context.SaveChanges();
            
            Console.WriteLine("Clinic deleted successfully.");
        }
    }
}
