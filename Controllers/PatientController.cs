using Microsoft.AspNetCore.Mvc;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Models;
using ClinicFlow.Dto;

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
                DateOfBirth = patientRequest.DateOfBirth,
                Email = patientRequest.Email,
                PhoneNumber = patientRequest.PhoneNumber
            };
            patientService.AddPatient(patient);

            var patientResponse = new PatientResponse
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
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
                DateOfBirth = patientRequest.DateOfBirth,
                Email = patientRequest.Email,
                PhoneNumber = patientRequest.PhoneNumber
            };

            patientService.UpdatePatient(patient);

            var patientResponse = new PatientResponse
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
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
