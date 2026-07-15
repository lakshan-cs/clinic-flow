namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IPatientAllergyService
{
    void AddPatientAllergy(PatientAllergy patientAllergy);
    IEnumerable<PatientAllergy> GetPatientAllergies();
    PatientAllergy GetPatientAllergy(int id);
    IEnumerable<PatientAllergy> GetPatientAllergiesByPatientId(int patientId);
    void UpdatePatientAllergy(PatientAllergy patientAllergy);
    void DeletePatientAllergy(int id);
}
