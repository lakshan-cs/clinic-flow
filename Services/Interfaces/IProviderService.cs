namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IProviderService
{
    void AddProvider(Provider provider);
    IEnumerable<Provider> GetProviders();
    IEnumerable<Provider> GetProvidersByClinicId(int clinicId);
    Provider GetProvider(int id);
    void UpdateProvider(Provider provider);
    void DeleteProvider(int id);
}
