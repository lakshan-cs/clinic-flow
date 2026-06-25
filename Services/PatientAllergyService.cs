using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;

namespace ClinicFlow.Services
{
    public class PatientAllergyService : IPatientAllergyService
    {
        private readonly IPatientAllergyRepository patientAllergyRepository;
        private readonly IPatientRepository patientRepository;
        private readonly IAllergyRepository allergyRepository;

        public PatientAllergyService(
            IPatientAllergyRepository patientAllergyRepository,
            IPatientRepository patientRepository,
            IAllergyRepository allergyRepository)
        {
            this.patientAllergyRepository = patientAllergyRepository;
            this.patientRepository = patientRepository;
            this.allergyRepository = allergyRepository;
        }

        public void AddPatientAllergy(PatientAllergy patientAllergy)
        {
            if (patientRepository.GetPatientById(patientAllergy.PatientId) == null)
                throw new NotFoundException("Patient not found with ID: " + patientAllergy.PatientId);

            if (allergyRepository.GetAllergyById(patientAllergy.AllergyId) == null)
                throw new NotFoundException("Allergy not found with ID: " + patientAllergy.AllergyId);

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
            if (patientRepository.GetPatientById(patientId) == null)
                throw new NotFoundException("Patient not found with ID: " + patientId);
            return patientAllergyRepository.GetPatientAllergiesByPatientId(patientId);
        }

        public IEnumerable<PatientAllergy> GetPatientAllergiesByAllergyId(int allergyId)
        {
            if (allergyRepository.GetAllergyById(allergyId) == null)
                throw new NotFoundException("Allergy not found with ID: " + allergyId);
            return patientAllergyRepository.GetPatientAllergiesByAllergyId(allergyId);
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
