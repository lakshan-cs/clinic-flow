using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;

namespace ClinicFlow.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public void AddPatient(Patient patient) 
        {
           var existingPatient = patientRepository.GetPatients()
           .FirstOrDefault(p => p.Email == patient.Email);

           if (existingPatient != null)
           {
               throw new DuplicateResourceException("A patient with the same email already exists: " + patient.Email);
           }
            
           patientRepository.AddPatient(patient);
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
