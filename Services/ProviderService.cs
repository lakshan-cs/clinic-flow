using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;

namespace ClinicFlow.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository providerRepository;
        private readonly IClinicRepository ClinicRepository;

        public ProviderService(IProviderRepository providerRepository, IClinicRepository clinicRepository)
        {
            this.providerRepository = providerRepository;
            this.ClinicRepository = clinicRepository;
        }

        public void AddProvider(Provider provider)
        {
            providerRepository.AddProvider(provider);
        }

        public IEnumerable<Provider> GetProviders()
        {
            return providerRepository.GetProviders();
        }

        public IEnumerable<Provider> GetProvidersByClinicId(int clinicId)
        {
            if (ClinicRepository.GetClinicById(clinicId) == null)
            {
                throw new ArgumentException("Clinic not found with ID: " + clinicId);
            }

            return providerRepository.GetProvidersByClinicId(clinicId);
        }

        public Provider GetProvider(int id)
        {
             var provider = providerRepository.GetProviderById(id);
             if (provider == null)
             {
                 throw new ArgumentException("Provider not found with ID: " + id);
             }
             return provider;
        }

        public void UpdateProvider(Provider provider)
        {
            var existingProvider = providerRepository.GetProviderById(provider.Id);
            if (existingProvider == null)
            {
                throw new ArgumentException("Provider not found with ID: " + provider.Id);
            }

            existingProvider.Name = provider.Name;
            existingProvider.Speciality = provider.Speciality;
            existingProvider.ClinicId = provider.ClinicId;

            providerRepository.UpdateProvider(existingProvider);
        }

        public void DeleteProvider(int id)
        {
            if (providerRepository.GetProviderById(id) != null)
                providerRepository.DeleteProvider(id);
            else
                throw new ArgumentException("Provider not found with ID: " + id);
        }
    }
}
