using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;

namespace ClinicFlow.Services
{
    public class PatientAllergyService : IPatientAllergyService
    {
        private readonly IPatientAllergyRepository patientAllergyRepository;

        public PatientAllergyService(IPatientAllergyRepository patientAllergyRepository)
        {
            this.patientAllergyRepository = patientAllergyRepository;
        }

        public void AddPatientAllergy(PatientAllergy patientAllergy)
        {
            patientAllergyRepository.AddPatientAllergy(patientAllergy);
        }

        public IEnumerable<PatientAllergy> GetPatientAllergies()
        {
            return patientAllergyRepository.GetPatientAllergies();
        }

        public PatientAllergy GetPatientAllergy(int id)
        {
            var pa = patientAllergyRepository.GetPatientAllergyById(id);
            if (pa == null)
                throw new NotFoundException("PatientAllergy not found with ID: " + id);
            return pa;
        }

        public IEnumerable<PatientAllergy> GetPatientAllergiesByPatientId(int patientId)
        {
            return patientAllergyRepository.GetPatientAllergiesByPatientId(patientId);
        }

        public void UpdatePatientAllergy(PatientAllergy patientAllergy)
        {
            var existing = patientAllergyRepository.GetPatientAllergyById(patientAllergy.Id);
            if (existing == null)
                throw new NotFoundException("PatientAllergy not found with ID: " + patientAllergy.Id);

            existing.Severity = patientAllergy.Severity;
            existing.Notes = patientAllergy.Notes;
            existing.PatientId = patientAllergy.PatientId;
            existing.AllergyId = patientAllergy.AllergyId;

            patientAllergyRepository.UpdatePatientAllergy(existing);
        }

        public void DeletePatientAllergy(int id)
        {
            if (patientAllergyRepository.GetPatientAllergyById(id) != null)
                patientAllergyRepository.DeletePatientAllergy(id);
            else
                throw new NotFoundException("PatientAllergy not found with ID: " + id);
        }
    }
}
