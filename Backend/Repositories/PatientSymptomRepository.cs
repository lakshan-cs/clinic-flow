using ClinicFlow.Data;
using ClinicFlow.Models;

namespace ClinicFlow.Repositories
{
    public class PatientSymptomRepository : IPatientSymptomRepository
    {
        private readonly ClinicDbContext context;

        public PatientSymptomRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<PatientSymptom> GetPatientSymptomsByPatientIntakeId(int patientIntakeId)
        {
            return context.PatientSymptoms.Where(ps => ps.PatientIntakeId == patientIntakeId).ToList();
        }

        public void AddPatientSymptom(PatientSymptom patientSymptom)
        {
            context.PatientSymptoms.Add(patientSymptom);
            context.SaveChanges();
        }
    }
}
