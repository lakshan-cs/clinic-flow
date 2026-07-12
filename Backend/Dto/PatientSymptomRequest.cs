namespace ClinicFlow.Dto;

using System.ComponentModel.DataAnnotations;

public class PatientSymptomRequest
{
    [Required]
    public int PatientIntakeId { get; set; }

    [Required]
    public string? Symptom { get; set; }
}
