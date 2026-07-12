namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IPatientIntakeService
{
    void AddPatientIntakeWithSymptoms(PatientIntake patientIntake, IEnumerable<PatientSymptom> patientSymptoms);

    PatientIntake GetPatientIntakeByPatientId(int patientId);
}
