namespace ClinicFlow.Models;

using System.ComponentModel.DataAnnotations.Schema;

[Table("CHIEF_COMPLAINT_KEYWORD_LOOKUP")]
public class ChiefComplaintKeywordLookup
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("KEYWORD")]
    public string? Keyword { get; set; }

    [Column("SPECIALTY")]
    public string? Specialty { get; set; }
}