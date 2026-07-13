using ClinicFlow.Models;
using ClinicFlow.Exceptions;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services.Interfaces;

namespace ClinicFlow.Services
{
    public class ChiefComplaintSpecialtyLookupService : IChiefComplaintSpecialtyLookupService
    {
        private readonly IChiefComplaintSpecialtyLookupRepository chiefComplaintSpecialtyLookupRepository;

        public ChiefComplaintSpecialtyLookupService(IChiefComplaintSpecialtyLookupRepository chiefComplaintSpecialtyLookupRepository)
        {
            this.chiefComplaintSpecialtyLookupRepository = chiefComplaintSpecialtyLookupRepository;
        }

        public IEnumerable<ChiefComplaintSpecialtyLookup> GetChiefComplaintSpecialtyLookups()
        {
            return chiefComplaintSpecialtyLookupRepository.GetChiefComplaintSpecialtyLookups();
        }

        public ChiefComplaintSpecialtyLookup? GetChiefComplaintSpecialityLookupByChiefComplaint(string chiefComplaint)
        {
            var lookup = chiefComplaintSpecialtyLookupRepository.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint);
            if (lookup == null)
            {
                throw new NotFoundException("Chief complaint specialty lookup not found for chief complaint: " + chiefComplaint);
            }

            return lookup;
        }
    }
}