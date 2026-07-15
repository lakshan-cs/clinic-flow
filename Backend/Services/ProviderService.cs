using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;

namespace ClinicFlow.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository providerRepository;

        public ProviderService(IProviderRepository providerRepository)
        {
            this.providerRepository = providerRepository;
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
            if (providerRepository.GetProvidersByClinicId(clinicId) == null)
            {
                throw new NotFoundException("No providers found for clinic with ID: " + clinicId);
            }
            return providerRepository.GetProvidersByClinicId(clinicId);
        }

        public IEnumerable<Provider> GetProvidersBySpeciality(string speciality)
        {
            var providers = providerRepository.GetProvidersBySpeciality(speciality);
            if (providers == null || !providers.Any())
            {
                throw new NotFoundException("No providers found with speciality: " + speciality);
            }
            return providers;
        }

        public Provider GetProvider(int id)
        {
            var provider = providerRepository.GetProviderById(id);
            if (provider == null)
            {
                throw new NotFoundException("Provider not found with ID: " + id);
            }
            return provider;
        }

        public void UpdateProvider(Provider provider)
        {
            var existingProvider = providerRepository.GetProviderById(provider.Id);
            if (existingProvider == null)
            {
                throw new NotFoundException("Provider not found with ID: " + provider.Id);
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
                throw new NotFoundException("Provider not found with ID: " + id);
        }
    }
}
