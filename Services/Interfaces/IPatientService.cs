namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IPatientService
{
    void AddPatient(Patient patient);
    IEnumerable<Patient> GetPatients();
    Patient GetPatient(int id);
    void UpdatePatient(Patient patient);
    void DeletePatient(int id);
}

