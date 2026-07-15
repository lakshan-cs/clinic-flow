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
        private readonly IPatientSymptomService patientSymptomService;
        private readonly ClinicDbContext dbContext;

        public PatientIntakeService(
            IPatientIntakeRepository patientIntakeRepository,
            IPatientSymptomService patientSymptomService,
            ClinicDbContext dbContext
        )
        {
            this.patientIntakeRepository = patientIntakeRepository;
            this.patientSymptomService = patientSymptomService;
            this.dbContext = dbContext;
        }

        public void AddPatientIntakeWithSymptoms(PatientIntake patientIntake, IEnumerable<PatientSymptom> patientSymptoms)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    patientIntakeRepository.AddPatientIntake(patientIntake);

                    foreach (var symptom in patientSymptoms)
                    {
                        symptom.PatientIntakeId = patientIntake.Id;
                        patientSymptomService.AddPatientSymptom(symptom);
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
            var patientIntake = patientIntakeRepository.GetPatientIntakeByPatientId(patientId);
            if (patientIntake == null)
                throw new NotFoundException("Patient intake not found for patient with ID: " + patientId);

            return patientIntake;
        }
    }
}
