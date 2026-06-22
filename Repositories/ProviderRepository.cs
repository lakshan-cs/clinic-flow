using ClinicFlow.Data;
using ClinicFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly ClinicDbContext context;

        public ProviderRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Provider> GetProviders()
        {
            return context.Providers
                .Include(p => p.Clinic)
                .ToList();
        }

        public Provider GetProviderById(int id)
        {
            return context.Providers
                .Include(p => p.Clinic)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Provider> GetProvidersByClinicId(int clinicId)
        {
            return context.Providers
                .Where(p => p.ClinicId == clinicId)
                .Include(p => p.Clinic)
                .ToList();
        }

        public void AddProvider(Provider provider)
        {
            context.Providers.Add(provider);
            context.SaveChanges();
        }

        public void UpdateProvider(Provider provider)
        {
            context.Providers.Find(provider.Id);
            context.SaveChanges();
        }

        public void DeleteProvider(int providerID)
        {
            Provider provider = context.Providers.Find(providerID);
            if (provider == null) return;
            context.Providers.Remove(provider);
            context.SaveChanges();

            Console.WriteLine("Provider deleted successfully.");
        }
    }
}
