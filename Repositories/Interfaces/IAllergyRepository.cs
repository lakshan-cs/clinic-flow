namespace ClinicFlow.Repositories;

using ClinicFlow.Models;

public interface IAllergyRepository
{
    IEnumerable<Allergy> GetAllergies();
    Allergy GetAllergyById(int id);
    void AddAllergy(Allergy allergy);
    void UpdateAllergy(Allergy allergy);
    void DeleteAllergy(int id);
}
