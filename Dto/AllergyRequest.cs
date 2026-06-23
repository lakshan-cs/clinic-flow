namespace ClinicFlow.Dto;

using System.ComponentModel.DataAnnotations;

public class AllergyRequest
{
    public int Id { get; set; }

    [Required]
    public string AllergyType { get; set; } = null!;
}
