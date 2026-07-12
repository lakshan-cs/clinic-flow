namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations.Schema;

[Table("PATIENT_SYMPTOM")]
public class PatientSymptom
{

    [Column("ID")]
    public int Id { get; set; }

    [Column("SYMPTOM")]
    public string? Symptom { get; set; }

    [Column("PATIENT_INTAKE_ID")]
    public int PatientIntakeId { get; set; }

    public PatientIntake? PatientIntake { get; set; }

}
