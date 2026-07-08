namespace ClinicFlow.Repositories;

using ClinicFlow.Models;

public interface IPatientAllergyRepository
{
    IEnumerable<PatientAllergy> GetPatientAllergies();
    PatientAllergy GetPatientAllergyById(int id);
    IEnumerable<PatientAllergy> GetPatientAllergiesByPatientId(int patientId);
    IEnumerable<PatientAllergy> GetPatientAllergiesByAllergyId(int allergyId);
    void AddPatientAllergy(PatientAllergy patientAllergy);
    void UpdatePatientAllergy(PatientAllergy patientAllergy);
    void DeletePatientAllergy(int id);
}
