namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CLINIC")]
public class Clinic
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("NAME")]
    public string? Name { get; set; }

    [Column("LOCATION")]
    public string? Location { get; set; }
}
