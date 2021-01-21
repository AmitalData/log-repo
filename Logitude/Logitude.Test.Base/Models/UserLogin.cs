namespace Logitude.Test.Base.Models
{
    public class UserLogin
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Tenant { get; set; }
        public string Token { get; set; }
        public bool InvalidToken { get; set; }
    }
}