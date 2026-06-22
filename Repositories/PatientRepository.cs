using ClinicFlow.Data;
using ClinicFlow.Models;

namespace ClinicFlow.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ClinicDbContext context;

        public PatientRepository(ClinicDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Patient> GetPatients()
        {
            return context.Patients.ToList();
        }

        public Patient GetPatientById(int id)
        {
            return context.Patients.Find(id);
        }

        public void AddPatient(Patient patient)
        {
            context.Patients.Add(patient);
            context.SaveChanges();
        }

        public void UpdatePatient(Patient patient)
        {
            context.Patients.Find(patient.Id);
            context.SaveChanges();
        }

        public void DeletePatient(int patientID)
        {
            var patient = context.Patients.Find(patientID);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                context.SaveChanges();
            }
            Console.WriteLine("Patient deleted successfully.");

        }

    }
}
