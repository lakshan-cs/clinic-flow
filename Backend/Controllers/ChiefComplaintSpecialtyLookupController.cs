using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiefComplaintSpecialtyLookupController : ControllerBase
    {
        private readonly IChiefComplaintSpecialtyLookupService chiefComplaintSpecialtyLookupService;

        public ChiefComplaintSpecialtyLookupController(IChiefComplaintSpecialtyLookupService chiefComplaintSpecialtyLookupService)
        {
            this.chiefComplaintSpecialtyLookupService = chiefComplaintSpecialtyLookupService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ChiefComplaintSpecialtyLookup>> GetChiefComplaintSpecialtyLookups()
        {
            var lookups = chiefComplaintSpecialtyLookupService.GetChiefComplaintSpecialtyLookups();
            return Ok(lookups);
        }

        [HttpGet("chief-complaint/{chiefComplaint}")]
        public ChiefComplaintSpecialtyLookup? GetChiefComplaintSpecialityLookupByChiefComplaint(string chiefComplaint)
        {
            return chiefComplaintSpecialtyLookupService.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint);
           
        }
    }
}