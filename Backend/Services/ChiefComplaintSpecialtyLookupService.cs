using ClinicFlow.Models;
using ClinicFlow.Exceptions;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services.Interfaces;

namespace ClinicFlow.Services
{
    public class ChiefComplaintSpecialtyLookupService : IChiefComplaintSpecialtyLookupService
    {
        private readonly IChiefComplaintSpecialtyLookupRepository chiefComplaintSpecialtyLookupRepository;
        private readonly IChiefComplaintKeywordLookupService chiefComplaintKeywordLookupService;

        public ChiefComplaintSpecialtyLookupService(IChiefComplaintSpecialtyLookupRepository chiefComplaintSpecialtyLookupRepository,
                                                    IChiefComplaintKeywordLookupService chiefComplaintKeywordLookupService
        )
        {
            this.chiefComplaintSpecialtyLookupRepository = chiefComplaintSpecialtyLookupRepository;
            this.chiefComplaintKeywordLookupService = chiefComplaintKeywordLookupService;
        }

        public IEnumerable<ChiefComplaintSpecialtyLookup> GetChiefComplaintSpecialtyLookups()
        {
            return chiefComplaintSpecialtyLookupRepository.GetChiefComplaintSpecialtyLookups();
        }

        public ChiefComplaintSpecialtyLookup? GetChiefComplaintSpecialityLookupByChiefComplaint(string chiefComplaint)
        {
            // Exact match
            var lookup = chiefComplaintSpecialtyLookupRepository.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint);

            if(lookup != null)
                return lookup;
            else
            {
                // Keyword match
                var words = chiefComplaint.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    var specialty = chiefComplaintKeywordLookupService.GetSpecialtyByKeyword(word);
                    if (!string.IsNullOrWhiteSpace(specialty))
                    {
                        return new ChiefComplaintSpecialtyLookup 
                        {
                            ChiefComplaint = chiefComplaint, 
                            Specialty = specialty 
                        };
                    }
                }

                // No match found
                throw new NotFoundException("Chief complaint specialty lookup not found for chief complaint: " + chiefComplaint);
            }
            
        }
    }
}