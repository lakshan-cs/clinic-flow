namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PROVIDER")]
public class Provider
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("NAME")]
    public string? Name { get; set; }

    [Column("SPECIALITY")]
    public string? Speciality { get; set; }

    [Column("CLINIC_ID")]
    public int ClinicId { get; set; }

    public Clinic? Clinic { get; set; }
}
