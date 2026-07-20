using ClinicFlow.Models;
using ClinicFlow.Exceptions;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services.Interfaces;

namespace ClinicFlow.Services
{
    public class ChiefComplaintKeywordLookupService : IChiefComplaintKeywordLookupService
    {
        private readonly IChiefComplaintKeywordLookupRepository chiefComplaintKeywordLookupRepository;

        public ChiefComplaintKeywordLookupService(IChiefComplaintKeywordLookupRepository chiefComplaintKeywordLookupRepository)
        {
            this.chiefComplaintKeywordLookupRepository = chiefComplaintKeywordLookupRepository;
        }

        public string? GetSpecialtyByKeyword(string keyword)
        {
            var lookup = chiefComplaintKeywordLookupRepository.GetChiefComplaintKeywordLookupByKeyword(keyword);
            return lookup?.Specialty;
        }
    }
}