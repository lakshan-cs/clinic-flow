using Microsoft.AspNetCore.Mvc;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Models;
using ClinicFlow.Dto;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService clinicService;

        public ClinicController(IClinicService clinicService)
        {
            this.clinicService = clinicService;
        }

        [HttpGet]
        public IEnumerable<Clinic> GetClinics()
        {
            return clinicService.GetClinics();
        }

        [HttpGet("{id}")]
        public ActionResult<Clinic> GetClinic(int id)
        {
            var clinic = clinicService.GetClinic(id);
            if (clinic == null)
            {
                return NotFound();
            }
            return clinic;
        }

        [HttpPost]
        public ActionResult<ClinicResponse> AddClinic([FromBody] ClinicRequest clinicRequest)
        {
            var clinic = new Clinic
            {
                Name = clinicRequest.Name,
                Location = clinicRequest.Location
            };
            clinicService.AddClinic(clinic);

            var clinicResponse = new ClinicResponse
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Location = clinic.Location
            };
            return CreatedAtAction(
                nameof(GetClinic), 
                new { id = clinic.Id }, 
                clinicResponse
            );

        }

        [HttpPut]
        public ActionResult<ClinicResponse> UpdateClinic([FromBody] ClinicRequest clinicRequest)
        {
            var clinic = new Clinic
            {
                Id = clinicRequest.Id,
                Name = clinicRequest.Name,
                Location = clinicRequest.Location
            };

            clinicService.UpdateClinic(clinic);
            var clinicResponse = new ClinicResponse
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Location = clinic.Location
            };
            return Ok(clinicResponse);
        }

        [HttpDelete("{id}")]
        public void DeleteClinic(int id)
        {
            clinicService.DeleteClinic(id);
        }

    }
}
