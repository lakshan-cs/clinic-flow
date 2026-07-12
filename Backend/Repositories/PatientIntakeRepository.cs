using ClinicFlow.Data;
using ClinicFlow.Models;

namespace ClinicFlow.Repositories
{
    public class PatientIntakeRepository : IPatientIntakeRepository
    {
        private readonly ClinicDbContext context;

        public PatientIntakeRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public PatientIntake GetPatientIntakeByPatientId(int patientId)
        {
            return context.PatientIntakes.FirstOrDefault(pi => pi.PatientId == patientId);
        }

        public void AddPatientIntake(PatientIntake patientIntake)
        {
            context.PatientIntakes.Add(patientIntake);
            context.SaveChanges();
        }
    }
}
