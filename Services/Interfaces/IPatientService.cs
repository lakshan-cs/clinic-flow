namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IPatientService
{
    void AddPatientWithAllergies(Patient patient, IEnumerable<PatientAllergy> patientAllergies);
    IEnumerable<Patient> GetPatients();
    Patient GetPatient(int id);
    void UpdatePatientWithAllergies(Patient patient, IEnumerable<PatientAllergy> patientAllergies);
    void DeletePatient(int id);
}

