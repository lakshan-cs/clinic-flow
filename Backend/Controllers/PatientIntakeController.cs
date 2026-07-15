using ClinicFlow.Dto;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientIntakeController : ControllerBase
    {
        private readonly IPatientIntakeService patientIntakeService;

        public PatientIntakeController(
            IPatientIntakeService patientIntakeService
        )
        {
            this.patientIntakeService = patientIntakeService;
        }

        // Add a new patient intake along with associated symptoms
        [HttpPost]
        public ActionResult<PatientIntakeResponse> AddPatientIntake([FromBody] PatientIntakeRequest request)
        {
            var patientIntake = new PatientIntake
            {
                PatientId = request.PatientId,
                ChiefComplaint = request.ChiefComplaint,
                Notes = request.Notes
            };

            var patientSymptoms = new List<PatientSymptom>();
            foreach (var symptomRequest in request.PatientSymptoms ?? new List<PatientSymptomRequest>())
            {
                patientSymptoms.Add(new PatientSymptom
                {
                    PatientIntakeId = patientIntake.Id,
                    Symptom = symptomRequest.Symptom
                });
        
            }
            patientIntakeService.AddPatientIntakeWithSymptoms(patientIntake, patientSymptoms);

            var response = new PatientIntakeResponse
            {
                Id = patientIntake.Id,
                PatientId = patientIntake.PatientId,
                ChiefComplaint = patientIntake.ChiefComplaint,
                Notes = patientIntake.Notes
            };

            return CreatedAtAction(
                nameof(GetPatientIntakeByPatientId),
                new { patientId = patientIntake.PatientId },
                response
            );
        }

        // Get patient intake by patient ID
        [HttpGet("patient/{patientId}")]
        public ActionResult<PatientIntakeResponse> GetPatientIntakeByPatientId(int patientId)
        {
            var patientIntake = patientIntakeService.GetPatientIntakeByPatientId(patientId);

            var response = new PatientIntakeResponse
            {
                Id = patientIntake.Id,
                PatientId = patientIntake.PatientId,
                ChiefComplaint = patientIntake.ChiefComplaint,
                Notes = patientIntake.Notes
            };

            return Ok(response);
        }
    }
}
