namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IClinicService
{
    void AddClinic(Clinic clinic);
    IEnumerable<Clinic> GetClinics();
    Clinic GetClinic(int id);
    void UpdateClinic(Clinic clinic);
    void DeleteClinic(int id);
}
