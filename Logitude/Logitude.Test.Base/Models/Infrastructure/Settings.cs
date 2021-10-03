using Logitude.Test.Base.Models.UserTenantPreparation;

namespace Logitude.Test.Base.Models.Infrastructure
{
    public static class Settings
    {
        public static string ServerUrl { get; set; }
        public static Credentials DefaultUserCredentials { get; set; }
        public static Credentials OtherUserCredentials { get; set; }
        public static Credentials UserEmptyTenantCredentials { get; set; }
    }
}