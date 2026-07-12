namespace ClinicFlow.Repositories;

using ClinicFlow.Models;

public interface IPatientSymptomRepository
{
    IEnumerable<PatientSymptom> GetPatientSymptomsByPatientIntakeId(int patientIntakeId);
    void AddPatientSymptom(PatientSymptom patientSymptom);
    
}
