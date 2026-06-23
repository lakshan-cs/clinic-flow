namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations.Schema;

[Table("ALLERGIES")]
public class Allergy
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("ALLERGY_TYPE")]
    public string? AllergyType { get; set; }
}
