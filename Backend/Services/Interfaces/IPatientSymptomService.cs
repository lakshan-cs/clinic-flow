namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IPatientSymptomService
{
    IEnumerable<PatientSymptom> GetPatientSymptomsByPatientIntakeId(int patientIntakeId);
}
