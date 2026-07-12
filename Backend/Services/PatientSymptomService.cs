using ClinicFlow.Exceptions;
using ClinicFlow.Models;
using ClinicFlow.Repositories;
using ClinicFlow.Services.Interfaces;

namespace ClinicFlow.Services
{
    public class PatientSymptomService : IPatientSymptomService
    {
        private readonly IPatientSymptomRepository patientSymptomRepository;

        public PatientSymptomService(IPatientSymptomRepository patientSymptomRepository)
        {
            this.patientSymptomRepository = patientSymptomRepository;
        }

        public IEnumerable<PatientSymptom> GetPatientSymptomsByPatientIntakeId(int patientIntakeId)
        {
            var patientSymptoms = patientSymptomRepository.GetPatientSymptomsByPatientIntakeId(patientIntakeId);
            if (patientSymptoms == null || !patientSymptoms.Any())
                throw new NotFoundException("Patient symptoms not found for intake with ID: " + patientIntakeId);

            return patientSymptoms;
        }
    }
}
