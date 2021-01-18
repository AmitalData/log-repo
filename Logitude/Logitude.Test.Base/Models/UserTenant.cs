namespace Logitude.Test.Base.Models
{
    public class UserTenant
    {
        public static string Token { get; set; }
        public static int Tenant { get; set; }
        public static string BranchId { get; set; }
        public static string DepartmentId { get; set; }
        public static string BusinessUnitId { get; set; }
        public static string ProfitCurrencyId { get; set; }
        public static double? ProfitCurrencyRate { get; set; }
        public static string LocalCurrencyId { get; set; }
        public static string LoginUserId { get; set; }
        public static string LoginUserName { get; set; }
    }
}
