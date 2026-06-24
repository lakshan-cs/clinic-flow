namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IPatientService
{
    void AddPatientWithAllergies(Patient patient, IEnumerable<PatientAllergy> patientAllergies);
    IEnumerable<Patient> GetPatients();
    Patient GetPatient(int id);
    void UpdatePatient(Patient patient);
    void DeletePatient(int id);
}

