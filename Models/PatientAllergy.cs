using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow.Models
{
    [Table("PATIENT_ALLERGY")]
    public class PatientAllergy
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("SEVERITY")]
        public string Severity { get; set; } = null!;

        [Column("NOTES")]
        public string? Notes { get; set; }

        [Column("PATIENT_ID")]
        public int PatientId { get; set; }
        
        public Patient Patient { get; set; } = null!;

        [Column("ALLERGY_TYPE_ID")]
        public int AllergyId { get; set; } 

        public Allergy Allergy { get; set; }
    }
}
