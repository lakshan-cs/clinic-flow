using Microsoft.AspNetCore.Mvc;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Models;
using ClinicFlow.Dto;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService providerService;

        public ProviderController(IProviderService providerService)
        {
            this.providerService = providerService;
        }

        [HttpGet]
        public IEnumerable<Provider> GetProviders()
        {
            return providerService.GetProviders();
        }

        [HttpGet("clinic/{clinicId}")]
        public IEnumerable<Provider> GetProvidersByClinicId(int clinicId)
        {
            return providerService.GetProvidersByClinicId(clinicId);
        }

        [HttpGet("{id}")]
        public ActionResult<Provider> GetProvider(int id)
        {
            return providerService.GetProvider(id);
           
        }

        [HttpPost]
        public ActionResult<ProviderResponse> AddProvider([FromBody] ProviderRequest providerRequest)
        {
            var provider = new Provider
            {
                Name = providerRequest.Name,
                Speciality = providerRequest.Speciality,
                ClinicId = providerRequest.ClinicId
            };
            providerService.AddProvider(provider);

            var providerResponse = new ProviderResponse
            {
                Id = provider.Id,
                Name = provider.Name,
                Speciality = provider.Speciality,
                ClinicId = provider.ClinicId
            };
            return CreatedAtAction(
                nameof(GetProvider), 
                new { id = provider.Id }, 
                providerResponse
            );

        }

        [HttpPut]
        public ActionResult<ProviderResponse> UpdateProvider([FromBody] ProviderRequest providerRequest)
        {
            var provider = new Provider
            {
                Id = providerRequest.Id,
                Name = providerRequest.Name,
                Speciality = providerRequest.Speciality,
                ClinicId = providerRequest.ClinicId
            };

            providerService.UpdateProvider(provider);
            var providerResponse = new ProviderResponse
            {
                Id = provider.Id,
                Name = provider.Name,
                Speciality = provider.Speciality,
                ClinicId = provider.ClinicId
            };
            return Ok(providerResponse);
        }

        [HttpDelete("{id}")]
        public void DeleteProvider(int id)
        {
            providerService.DeleteProvider(id);
        }

    }
}
