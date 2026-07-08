namespace ClinicFlow.Dto;

using System.ComponentModel.DataAnnotations;

public class ClinicRequest
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;
}
