namespace ClinicFlow.Dto;

public class PatientIntakeResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string? ChiefComplaint { get; set; }

    public string? Notes { get; set; }
}
