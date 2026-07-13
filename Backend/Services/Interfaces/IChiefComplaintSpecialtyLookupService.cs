using ClinicFlow.Models;

namespace ClinicFlow.Services.Interfaces
{
    public interface IChiefComplaintSpecialtyLookupService
    {
        IEnumerable<ChiefComplaintSpecialtyLookup> GetChiefComplaintSpecialtyLookups();
        ChiefComplaintSpecialtyLookup? GetChiefComplaintSpecialityLookupByChiefComplaint(string chiefComplaint);
    }
}