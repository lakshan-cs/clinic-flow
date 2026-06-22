namespace ClinicFlow.Repositories;

using ClinicFlow.Models;

public interface IClinicRepository
{
    IEnumerable<Clinic> GetClinics();
    Clinic GetClinicById(int id);
    void AddClinic(Clinic clinic);
    void UpdateClinic(Clinic clinic);
    void DeleteClinic(int id);
}
