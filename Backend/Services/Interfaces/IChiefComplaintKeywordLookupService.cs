using ClinicFlow.Models;

namespace ClinicFlow.Services.Interfaces
{
    public interface IChiefComplaintKeywordLookupService
    {
        string? GetSpecialtyByKeyword(string keyword);
    }
}