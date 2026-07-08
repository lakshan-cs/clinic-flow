namespace ClinicFlow.Dto;

public class PatientAllergyResponse
{
    public int Id { get; set; }

    public string? Severity { get; set; }

    public string? Notes { get; set; }

    public int PatientId { get; set; }

    public int AllergyId { get; set; }
}
