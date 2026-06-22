namespace ClinicFlow.Repositories;

using ClinicFlow.Models;

public interface IProviderRepository
{
    IEnumerable<Provider> GetProviders();
    IEnumerable<Provider> GetProvidersByClinicId(int clinicId);
    Provider GetProviderById(int id);
    void AddProvider(Provider provider);
    void UpdateProvider(Provider provider);
    void DeleteProvider(int id);
}
