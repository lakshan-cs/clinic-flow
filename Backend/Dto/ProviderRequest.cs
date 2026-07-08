namespace ClinicFlow.Dto;

using System.ComponentModel.DataAnnotations;

public class ProviderRequest
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Speciality { get; set; } = null!;

    [Required]
    public int ClinicId { get; set; }
}
