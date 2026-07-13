namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IProviderService
{
    void AddProvider(Provider provider);
    IEnumerable<Provider> GetProviders();
    IEnumerable<Provider> GetProvidersByClinicId(int clinicId);
    IEnumerable<Provider> GetProvidersBySpeciality(string speciality);
    Provider GetProvider(int id);
    void UpdateProvider(Provider provider);
    void DeleteProvider(int id);
}
