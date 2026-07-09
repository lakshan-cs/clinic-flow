namespace ClinicFlow.Exceptions
{
    public class ResourceInUseException : Exception
    {
        public ResourceInUseException(string message) : base(message) { }
    }
}
