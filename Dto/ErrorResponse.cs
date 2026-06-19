namespace ClinicFlow.Dto
{
    public class ErrorResponse
    {
        public string TimeStamp { get; set; } 
        public string Error { get; set; }
        public int StatusCode { get; set; }
        public string? Path { get; set; }

    }
}
