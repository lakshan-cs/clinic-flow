namespace ClinicFlow.Dto
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ClinicId { get; set; }
        public int ProviderId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Reason { get; set; }
    }
}
