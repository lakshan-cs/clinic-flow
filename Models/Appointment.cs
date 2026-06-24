using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicFlow.Models
{
    [Table("APPOINTMENTS")]
    public class Appointment
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("PATIENT_ID")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }
        
        [Column("CLINIC_ID")]
        public int ClinicId { get; set; }

        public Clinic Clinic { get; set; }
        
        [Column("PROVIDER_ID")]
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }
        
        [Column("APPOINTMENT_DATETIME")]
        public DateTime DateTime { get; set; }

        [Column("REASON_FOR_VISIT")]
        public string? Reason { get; set; }
    }
}
