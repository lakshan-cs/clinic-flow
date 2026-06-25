using Microsoft.AspNetCore.Mvc;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Models;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientAllergyController : ControllerBase
    {
        private readonly IPatientAllergyService patientAllergyService;

        public PatientAllergyController(IPatientAllergyService patientAllergyService)
        {
            this.patientAllergyService = patientAllergyService;
        }

        [HttpGet]
        public IEnumerable<PatientAllergy> GetPatientAllergies()
        {
            return patientAllergyService.GetPatientAllergies();
        }

        [HttpGet("{id}")]
        public ActionResult<PatientAllergy> GetPatientAllergy(int id)
        {
            var pa = patientAllergyService.GetPatientAllergy(id);
            if (pa == null)
                return NotFound();
            return pa;
        }

        [HttpGet("patient/{patientId}")]
        public IEnumerable<PatientAllergy> GetByPatient(int patientId)
        {
            return patientAllergyService.GetPatientAllergiesByPatientId(patientId);
        }

        [HttpGet("allergy/{allergyId}")]
        public IEnumerable<PatientAllergy> GetByAllergy(int allergyId)
        {
            return patientAllergyService.GetPatientAllergiesByAllergyId(allergyId);
        }
    }
}
