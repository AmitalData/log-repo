namespace Logitude.Test.Base.Models
{
    public class LoginParameters
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientType { get; set; }
        public bool GetToken { get; set; }
    }
}