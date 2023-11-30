namespace Logitude.Base.Models.UserTenant
{
    public static class UserTenant
    {
        public static int Tenant { get; set; }
        public static string Token { get; set; }
        public static string BranchId { get; set; }
        public static string DepartmentId { get; set; }
        public static string BusinessUnitId { get; set; }
        public static string ProfitCurrencyId { get; set; }
        public static double? ProfitCurrencyRate { get; set; }
        public static string LocalCurrencyId { get; set; }
        public static string UserId { get; set; }
        public static string DocumentDownloadToken { get; set; }
        public static string UserName { get; set; }
    }
}