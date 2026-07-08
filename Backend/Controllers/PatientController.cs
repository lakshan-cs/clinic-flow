using Microsoft.AspNetCore.Mvc;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Models;
using ClinicFlow.Dto;
using ClinicFlow.Repositories;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService patientService;

        public PatientController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        [HttpGet]
        public IEnumerable<Patient> GetPatients()
        {
            return patientService.GetPatients();
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatient(int id)
        {
            var patient = patientService.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        [HttpPost]
        public ActionResult<PatientResponse> AddPatient([FromBody] PatientRequest patientRequest)
        {
            var patient = new Patient
            {
                FullName = patientRequest.FullName,
                DateOfBirth = patientRequest.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                Email = patientRequest.Email,
                PhoneNumber = patientRequest.PhoneNumber
            };
            // Prepare patient allergies from request
            var patientAllergies = new List<PatientAllergy>();
            foreach (var allergyRequest in patientRequest.PatientAllergies)
            {
                patientAllergies.Add(new PatientAllergy
                {
                    AllergyId = allergyRequest.AllergyId,
                    Severity = allergyRequest.Severity ?? string.Empty,
                    Notes = allergyRequest.Notes
                });
            }

            // Add patient along with allergies atomically
            patientService.AddPatientWithAllergies(patient, patientAllergies);

            var patientResponse = new PatientResponse
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth?.ToString("yyyy-MM-dd"),
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber
            };

            return CreatedAtAction(
                nameof(GetPatient), 
                new { id = patient.Id }, 
                patientResponse
            );

        }

        [HttpPut]
        public ActionResult<PatientResponse> UpdatePatient([FromBody] PatientRequest patientRequest)
        {
            var patient = new Patient
            {
                Id = patientRequest.Id,
                FullName = patientRequest.FullName,
                DateOfBirth = patientRequest.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                Email = patientRequest.Email,
                PhoneNumber = patientRequest.PhoneNumber
            };

            var patientAllergies = new List<PatientAllergy>();
            foreach (var allergyRequest in patientRequest.PatientAllergies)
            {
                patientAllergies.Add(new PatientAllergy
                {
                    AllergyId = allergyRequest.AllergyId,
                    Severity = allergyRequest.Severity ?? string.Empty,
                    Notes = allergyRequest.Notes
                });
            }
            patientService.UpdatePatientWithAllergies(patient, patientAllergies);

            var patientResponse = new PatientResponse
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth?.ToString("yyyy-MM-dd"),
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber
            };

            return Ok(patientResponse);
        }

        [HttpDelete("{id}")]
        public void DeletePatient(int id)
        {
            patientService.DeletePatient(id);
        }

    }
}
