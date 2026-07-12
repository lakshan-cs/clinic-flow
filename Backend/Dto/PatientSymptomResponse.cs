namespace ClinicFlow.Dto;

public class PatientSymptomResponse
{
    public int Id { get; set; }

    public int PatientIntakeId { get; set; }

    public string? Symptom { get; set; }
}
