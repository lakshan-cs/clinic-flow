using ClinicFlow.Data;
using ClinicFlow.Models;
using ClinicFlow.Repositories.Interfaces;

namespace ClinicFlow.Repositories
{
    public class ChiefComplaintSpecialtyLookupRepository : IChiefComplaintSpecialtyLookupRepository
    {
        private readonly ClinicDbContext context;

        public ChiefComplaintSpecialtyLookupRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ChiefComplaintSpecialtyLookup> GetChiefComplaintSpecialtyLookups()
        {
            return context.ChiefComplaintSpecialtyLookups.ToList();
        }

        public ChiefComplaintSpecialtyLookup? GetChiefComplaintSpecialityLookupByChiefComplaint(string chiefComplaint)
        {
            return context.ChiefComplaintSpecialtyLookups
                .FirstOrDefault(x =>
                    x.ChiefComplaint.ToLower() ==
                    chiefComplaint.Trim().ToLower());
        }
    }
}