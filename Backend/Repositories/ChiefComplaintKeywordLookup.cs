using ClinicFlow.Data;
using ClinicFlow.Models;
using ClinicFlow.Repositories.Interfaces;
    
namespace ClinicFlow.Repositories
{
    public class ChiefComplaintKeywordLookupRepository : IChiefComplaintKeywordLookupRepository
    {
        private readonly ClinicDbContext context;

        public ChiefComplaintKeywordLookupRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public ChiefComplaintKeywordLookup? GetChiefComplaintKeywordLookupByKeyword(string keyword)
        {
            return context.ChiefComplaintKeywordLookups
                    .FirstOrDefault(x =>
                        x.Keyword.ToLower() == keyword.Trim().ToLower());
        }

    }
}