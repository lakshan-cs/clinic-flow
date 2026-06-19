namespace ClinicFlow.Dto;

public class PatientResponse
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
