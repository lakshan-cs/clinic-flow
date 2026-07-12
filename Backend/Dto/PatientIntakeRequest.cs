namespace ClinicFlow.Dto;

using System.ComponentModel.DataAnnotations;

public class PatientIntakeRequest
{
    [Required]
    public int PatientId { get; set; }

    [Required]
    public string? ChiefComplaint { get; set; }

    public string? Notes { get; set; }

    public List<PatientSymptomRequest>? PatientSymptoms { get; set; }
}
