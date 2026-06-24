using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.Dto
{
    public class AppointmentRequest
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ClinicId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public string? Reason { get; set; }
    }
}
