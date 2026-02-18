namespace AmitalCloud.Infrastructure.Data.Exceptions
{
    public class APIException
    {
        public string ErrorType { get; set; }
        public string ShortErrorMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
