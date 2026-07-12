namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations.Schema;

[Table("PATIENT_INTAKE")]
public class PatientIntake
{

    [Column("ID")]
    public int Id { get; set; }

    [Column("CHIEF_COMPLAINT")]
    public string? ChiefComplaint { get; set; }

    [Column("NOTES")]
    public string? Notes { get; set; }

    [Column("PATIENT_ID")]
    public int PatientId { get; set; }

    public Patient? Patient { get; set; }


}
