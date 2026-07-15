using ClinicFlow.Repositories;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;
using ClinicFlow.Data;

namespace ClinicFlow.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository patientRepository;
        private readonly IPatientAllergyService patientAllergyService;
        private readonly IAppointmentService appointmentService;
        private readonly ClinicDbContext dbContext;

        public PatientService(
            IPatientRepository patientRepository, 
            IPatientAllergyService patientAllergyService, 
            IAppointmentService appointmentService,
            ClinicDbContext dbContext
            )
        {
            this.patientRepository = patientRepository;
            this.patientAllergyService = patientAllergyService;
            this.appointmentService = appointmentService;
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
                        patientAllergy.PatientId = patient.Id;
                        IEnumerable<PatientAllergy> allergies = patientAllergyService
                            .GetPatientAllergiesByPatientId(patient.Id);

                        foreach(var existingAllergy in allergies)
                        {
                            if (existingAllergy.AllergyId == patientAllergy.AllergyId)
                            {
                                throw new DuplicateResourceException("The patient already has this allergy recorded: " + patientAllergy.AllergyId);
                            }
                        }
                        patientAllergyService.AddPatientAllergy(patientAllergy);
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
                throw new NotFoundException("Patient not found with ID: " + id);
            }
            return patient;
        }

        public void UpdatePatientWithAllergies(Patient patient, IEnumerable<PatientAllergy> patientAllergies)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var existingPatient = patientRepository.GetPatientById(patient.Id);
                    if (existingPatient == null)
                    {
                        throw new NotFoundException("Patient not found with ID: " + patient.Id);
                    }

                    existingPatient.FullName = patient.FullName;
                    existingPatient.DateOfBirth = patient.DateOfBirth;
                    existingPatient.Email = patient.Email;
                    existingPatient.PhoneNumber = patient.PhoneNumber;

                    patientRepository.UpdatePatient(existingPatient);

                    var allergyList = patientAllergies.ToList();
                    var submittedAllergyIds = allergyList.Select(a => a.AllergyId).ToHashSet();

                    var existingAllergies = patientAllergyService
                        .GetPatientAllergiesByPatientId(patient.Id).ToList();

                    // Remove allergies that are no longer in the submitted list
                    foreach (var existing in existingAllergies)
                    {
                        if (!submittedAllergyIds.Contains(existing.AllergyId))
                        {
                            patientAllergyService.DeletePatientAllergy(existing.Id);
                        }
                    }

                    // Add or update submitted allergies
                    foreach (var patientAllergy in allergyList)
                    {
                        patientAllergy.PatientId = patient.Id;
                        var existingAllergy = existingAllergies.FirstOrDefault(a => a.AllergyId == patientAllergy.AllergyId);

                        if (existingAllergy != null)
                        {
                            existingAllergy.Severity = patientAllergy.Severity;
                            existingAllergy.Notes = patientAllergy.Notes;
                            patientAllergyService.UpdatePatientAllergy(existingAllergy);
                        }
                        else
                        {
                            patientAllergyService.AddPatientAllergy(patientAllergy);
                        }
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

        public void DeletePatient(int id)
        {
            var patient = patientRepository.GetPatientById(id);
            if (patient == null)
            {
                throw new NotFoundException("Patient not found with ID: " + id);
            } 
            if (appointmentService.GetAppointmentsByPatientId(id).Any())
            {
                throw new ResourceInUseException("Cannot delete patient with existing appointments.");
            }
            patientRepository.DeletePatient(id);
        }
    }
}
