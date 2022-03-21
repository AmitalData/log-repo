using Logitude.Base.Models.UserTenantPreparation;

namespace Logitude.Base.Models.Infrastructure
{
    public static class Settings
    {
        public static string ServerUrl { get; set; }
        public static Credentials DefaultUserCredentials { get; set; }
        public static Credentials OtherUserCredentials { get; set; }
        public static Credentials UserEmptyTenantCredentials { get; set; }
    }
}