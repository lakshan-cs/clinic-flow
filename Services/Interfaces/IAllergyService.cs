namespace ClinicFlow.Services.Interfaces;

using ClinicFlow.Models;

public interface IAllergyService
{
    void AddAllergy(Allergy allergy);
    IEnumerable<Allergy> GetAllergies();
    Allergy GetAllergy(int id);
    void UpdateAllergy(Allergy allergy);
    void DeleteAllergy(int id);
}
