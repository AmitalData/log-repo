namespace MicrosoftGraphClient.Models.AuthenticationService
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string[] Scopes { get; set; }
    }
}