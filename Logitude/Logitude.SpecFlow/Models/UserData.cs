namespace Logitude.SpecFlow.Models
{
    public class UserData
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Tenant { get; set; }
        public string Token { get; set; }
        public bool InvalidToken { get; set; }
    }
}