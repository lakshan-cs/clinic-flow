namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PATIENTS")]
public class Patient
{

    [Column("ID")]
    public int Id { get; set; }
    [Column("FULL_NAME")]
    public string? FullName { get; set; }
    [Column("DOB")]
    public string? DateOfBirth { get; set; }
    [Column("EMAIL")]
    public string? Email { get; set; }
    [Column("PHONE_NUMBER")]
    public string? PhoneNumber { get; set; }
}
