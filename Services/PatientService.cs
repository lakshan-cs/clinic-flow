using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;
using ClinicFlow.Data;
using System.Numerics;

namespace ClinicFlow.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository patientRepository;
        private readonly IAllergyRepository allergyRepository;
        private readonly IPatientAllergyRepository patientAllergyRepository;
        private readonly ClinicDbContext dbContext;

        public PatientService(
            IPatientRepository patientRepository, 
            IAllergyRepository allergyRepository, 
            IPatientAllergyRepository patientAllergyRepository, 
            ClinicDbContext dbContext
            )
        {
            this.patientRepository = patientRepository;
            this.allergyRepository = allergyRepository;
            this.patientAllergyRepository = patientAllergyRepository;
            this.dbContext = dbContext;
        }
        
        public void AddPatientWithAllergies(Patient patient, IEnumerable<PatientAllergy> patientAllergies)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var existingPatient = patientRepository.GetPatients()
                        .FirstOrDefault(p => p.Email == patient.Email);

                    if (existingPatient != null)
                    {
                        throw new DuplicateResourceException("A patient with the same email already exists: " + patient.Email);
                    }
                    // Add the patient first
                    patientRepository.AddPatient(patient);
                    // Now add the allergies for the patient
                    foreach (var patientAllergy in patientAllergies)
                    {
                        // Ensure the allergy exists
                        var allergy = allergyRepository.GetAllergyById(patientAllergy.AllergyId);
                        if (allergy == null)
                        {
                            throw new ArgumentException("Allergy not found with ID: " + patientAllergy.AllergyId);
                        }
                        patientAllergy.PatientId = patient.Id;
                        IEnumerable<PatientAllergy> allergies = patientAllergyRepository
                            .GetPatientAllergiesByPatientId(patient.Id);

                        foreach(var existingAllergy in allergies)
                        {
                            if (existingAllergy.AllergyId == patientAllergy.AllergyId)
                            {
                                throw new DuplicateResourceException("The patient already has this allergy recorded: " + patientAllergy.AllergyId);
                            }
                        }
                        patientAllergyRepository.AddPatientAllergy(patientAllergy);
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
        public IEnumerable<Patient> GetPatients()
        {
            return patientRepository.GetPatients();
        }

        public Patient GetPatient(int id)
        {
            var patient = patientRepository.GetPatientById(id);
            if (patient == null)
            {
                throw new ArgumentException("Patient not found with ID: " + id);
            }
            return patient;
        }

        public void UpdatePatient(Patient patient)
        {
            var existingPatient = patientRepository.GetPatientById(patient.Id);

            if(existingPatient == null)
            {
                throw new ArgumentException("Patient not found with ID: " + patient.Id);
            }

            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Email = patient.Email;
            existingPatient.PhoneNumber = patient.PhoneNumber;

            patientRepository.UpdatePatient(existingPatient);
  

        }

        public void DeletePatient(int id)
        {
            var patient = patientRepository.GetPatientById(id);
            if (patient == null)
            {
                throw new ArgumentException("Patient not found with ID: " + id);
            }
            patientRepository.DeletePatient(id);
        }
    }
}
