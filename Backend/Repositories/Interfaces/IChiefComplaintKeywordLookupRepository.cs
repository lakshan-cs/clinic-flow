using ClinicFlow.Models;

namespace ClinicFlow.Repositories.Interfaces
{
    public interface IChiefComplaintKeywordLookupRepository
    {
        ChiefComplaintKeywordLookup? GetChiefComplaintKeywordLookupByKeyword(string keyword);
    }
}