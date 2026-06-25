using ClinicFlow.Repositories;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using ClinicFlow.Exceptions;

namespace ClinicFlow.Services
{
    public class AllergyService : IAllergyService
    {
        private readonly IAllergyRepository allergyRepository;

        public AllergyService(IAllergyRepository allergyRepository)
        {
            this.allergyRepository = allergyRepository;
        }

        public void AddAllergy(Allergy allergy)
        {
            allergyRepository.AddAllergy(allergy);
        }

        public IEnumerable<Allergy> GetAllergies()
        {
            return allergyRepository.GetAllergies();
        }

        public Allergy GetAllergy(int id)
        {
            var allergy = allergyRepository.GetAllergyById(id);
            if (allergy == null)
            {
                throw new NotFoundException("Allergy not found with ID: " + id);
            }
            return allergy;
        }

        public void UpdateAllergy(Allergy allergy)
        {
            var existing = allergyRepository.GetAllergyById(allergy.Id);
            if (existing == null)
            {
                throw new NotFoundException("Allergy not found with ID: " + allergy.Id);
            }

            existing.AllergyType = allergy.AllergyType;

            allergyRepository.UpdateAllergy(existing);
        }

        public void DeleteAllergy(int id)
        {
            if (allergyRepository.GetAllergyById(id) != null)
                allergyRepository.DeleteAllergy(id);
            else
                throw new NotFoundException("Allergy not found with ID: " + id);
        }
    }
}
