namespace ClinicFlow.Repositories;

using ClinicFlow.Models;

public interface IPatientIntakeRepository
{
    PatientIntake GetPatientIntakeByPatientId(int patientId);
    void AddPatientIntake(PatientIntake patientIntake);
    
}
