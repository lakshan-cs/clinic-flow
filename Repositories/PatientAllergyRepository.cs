using ClinicFlow.Data;
using ClinicFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Repositories
{
    public class PatientAllergyRepository : IPatientAllergyRepository
    {
        private readonly ClinicDbContext context;

        public PatientAllergyRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<PatientAllergy> GetPatientAllergies()
        {
            return context.PatientAllergies
                .Include(pa => pa.Patient)
                .Include(pa => pa.Allergy)
                .ToList();
        }

        public PatientAllergy GetPatientAllergyById(int id)
        {
            return context.PatientAllergies
                .Include(pa => pa.Patient)
                .Include(pa => pa.Allergy)
                .FirstOrDefault(pa => pa.Id == id);
        }

        public IEnumerable<PatientAllergy> GetPatientAllergiesByPatientId(int patientId)
        {
            return context.PatientAllergies
                .Where(pa => pa.PatientId == patientId)
                .Include(pa => pa.Allergy)
                .ToList();
        }

        public IEnumerable<PatientAllergy> GetPatientAllergiesByAllergyId(int allergyId)
        {
            return context.PatientAllergies
                .Where(pa => pa.AllergyId == allergyId)
                .Include(pa => pa.Patient)
                .ToList();
        }

        public void AddPatientAllergy(PatientAllergy patientAllergy)
        {
            context.PatientAllergies.Add(patientAllergy);
            context.SaveChanges();
        }

        public void UpdatePatientAllergy(PatientAllergy patientAllergy)
        {
            context.PatientAllergies.Find(patientAllergy.Id);
            context.SaveChanges();
        }

        public void DeletePatientAllergy(int id)
        {
            var pa = context.PatientAllergies.Find(id);
            if (pa == null) return;
            context.PatientAllergies.Remove(pa);
            context.SaveChanges();
        }
    }
}
