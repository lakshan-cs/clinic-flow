namespace ClinicFlow.Repositories;
using ClinicFlow.Models;

public interface IPatientRepository 
{
    IEnumerable<Patient> GetPatients();
    Patient GetPatientById(int id);
    void AddPatient(Patient patient);
    void UpdatePatient(Patient patient);
    void DeletePatient(int id);
}

