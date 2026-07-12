using ClinicFlow.Exceptions;
using ClinicFlow.Models;
using ClinicFlow.Repositories;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Data;

namespace ClinicFlow.Services
{
    public class PatientIntakeService : IPatientIntakeService
    {
        private readonly IPatientIntakeRepository patientIntakeRepository;
        private readonly IPatientSymptomRepository patientSymptomRepository;
        private readonly IPatientRepository patientRepository;
        private readonly ClinicDbContext dbContext;

        public PatientIntakeService(
            IPatientIntakeRepository patientIntakeRepository,
            IPatientSymptomRepository patientSymptomRepository,
            IPatientRepository patientRepository,
            ClinicDbContext dbContext
        )
        {
            this.patientIntakeRepository = patientIntakeRepository;
            this.patientSymptomRepository = patientSymptomRepository;
            this.patientRepository = patientRepository;
            this.dbContext = dbContext;
        }

        public void AddPatientIntakeWithSymptoms(PatientIntake patientIntake, IEnumerable<PatientSymptom> patientSymptoms)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (patientRepository.GetPatientById(patientIntake.PatientId) == null)
                        throw new NotFoundException("Patient not found with ID: " + patientIntake.PatientId);

                    patientIntakeRepository.AddPatientIntake(patientIntake);

                    foreach (var symptom in patientSymptoms)
                    {
                        symptom.PatientIntakeId = patientIntake.Id;
                        patientSymptomRepository.AddPatientSymptom(symptom);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public PatientIntake GetPatientIntakeByPatientId(int patientId)
        {
            if (patientRepository.GetPatientById(patientId) == null)
                throw new NotFoundException("Patient not found with ID: " + patientId);

            var patientIntake = patientIntakeRepository.GetPatientIntakeByPatientId(patientId);
            if (patientIntake == null)
                throw new NotFoundException("Patient intake not found for patient with ID: " + patientId);

            return patientIntake;
        }
    }
}
