using ClinicFlow.Models;

namespace ClinicFlow.Repositories.Interfaces
{
    public interface IChiefComplaintSpecialtyLookupRepository
    {
        IEnumerable<ChiefComplaintSpecialtyLookup> GetChiefComplaintSpecialtyLookups();
        ChiefComplaintSpecialtyLookup GetChiefComplaintSpecialityLookupByChiefComplaint(string chiefComplaint);
    }
}