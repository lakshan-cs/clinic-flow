using ClinicFlow.Data;
using ClinicFlow.Models;

namespace ClinicFlow.Repositories
{
    public class AllergyRepository : IAllergyRepository
    {
        private readonly ClinicDbContext context;

        public AllergyRepository(ClinicDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Allergy> GetAllergies()
        {
            return context.Allergies.ToList();
        }

        public Allergy GetAllergyById(int id)
        {
            return context.Allergies.Find(id);
        }

        public void AddAllergy(Allergy allergy)
        {
            context.Allergies.Add(allergy);
            context.SaveChanges();
        }

        public void UpdateAllergy(Allergy allergy)
        {
            context.Allergies.Find(allergy.Id);
            context.SaveChanges();
        }

        public void DeleteAllergy(int id)
        {
            Allergy allergy = context.Allergies.Find(id);
            if (allergy == null) return;
            context.Allergies.Remove(allergy);
            context.SaveChanges();

            Console.WriteLine("Allergy deleted successfully.");
        }
    }
}
