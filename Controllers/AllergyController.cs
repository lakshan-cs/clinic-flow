using Microsoft.AspNetCore.Mvc;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Models;
using ClinicFlow.Dto;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllergyController : ControllerBase
    {
        private readonly IAllergyService allergyService;

        public AllergyController(IAllergyService allergyService)
        {
            this.allergyService = allergyService;
        }

        [HttpGet]
        public IEnumerable<Allergy> GetAllergies()
        {
            return allergyService.GetAllergies();
        }

        [HttpGet("{id}")]
        public ActionResult<Allergy> GetAllergy(int id)
        {
            var allergy = allergyService.GetAllergy(id);
            if (allergy == null)
            {
                return NotFound();
            }
            return allergy;
        }

        [HttpPost]
        public ActionResult<AllergyResponse> AddAllergy([FromBody] AllergyRequest request)
        {
            var allergy = new Allergy
            {
                AllergyType = request.AllergyType
            };

            allergyService.AddAllergy(allergy);

            var response = new AllergyResponse
            {
                Id = allergy.Id,
                AllergyType = allergy.AllergyType
            };

            return CreatedAtAction(
                nameof(GetAllergy),
                new { id = allergy.Id },
                response
            );
        }

        [HttpPut]
        public ActionResult<AllergyResponse> UpdateAllergy([FromBody] AllergyRequest request)
        {
            var allergy = new Allergy
            {
                Id = request.Id,
                AllergyType = request.AllergyType
            };

            allergyService.UpdateAllergy(allergy);

            var response = new AllergyResponse
            {
                Id = allergy.Id,
                AllergyType = allergy.AllergyType
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public void DeleteAllergy(int id)
        {
            allergyService.DeleteAllergy(id);
        }
    }
}
