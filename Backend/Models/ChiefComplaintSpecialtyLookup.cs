namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations.Schema;

[Table("CHIEF_COMPLAINT_SPECIALTY_LOOKUP")]
public class ChiefComplaintSpecialtyLookup
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("CHIEF_COMPLAINT")]
    public string? ChiefComplaint { get; set; }

    [Column("SPECIALTY")]
    public string? Specialty { get; set; }
}