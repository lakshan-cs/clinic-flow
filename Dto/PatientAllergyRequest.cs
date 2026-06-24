namespace ClinicFlow.Dto;

using System.ComponentModel.DataAnnotations;

public class PatientAllergyRequest
{
    public int AllergyId { get; set; }
    public string? Severity { get; set; }

    public string? Notes { get; set; }
}
