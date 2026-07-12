using ClinicFlow.Dto;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientSymptomController : ControllerBase
    {
        private readonly IPatientSymptomService patientSymptomService;

        public PatientSymptomController(IPatientSymptomService patientSymptomService)
        {
            this.patientSymptomService = patientSymptomService;
        }

        [HttpGet("intake/{patientIntakeId}")]
        public ActionResult<IEnumerable<PatientSymptomResponse>> GetPatientSymptomsByPatientIntakeId(int patientIntakeId)
        {
            var patientSymptoms = patientSymptomService.GetPatientSymptomsByPatientIntakeId(patientIntakeId);

            var response = patientSymptoms.Select(ps => new PatientSymptomResponse
            {
                Id = ps.Id,
                PatientIntakeId = ps.PatientIntakeId,
                Symptom = ps.Symptom
            }).ToList();

            return Ok(response);
        }
    }
}

               
