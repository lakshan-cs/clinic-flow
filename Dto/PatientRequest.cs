namespace ClinicFlow.Dto;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class PatientRequest
{
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    public string DateOfBirth { get; set; } = null!;

    [EmailAddress]
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("allergies")]
    public List<PatientAllergyRequest> PatientAllergies { get; set; } 
        = new List<PatientAllergyRequest>();
}
