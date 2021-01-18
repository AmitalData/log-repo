namespace Logitude.Test.Base.Models.Base
{
    public class TenantPM
    {
        public int Id { get; set; }
        public string CurrencyId { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitCurrencyRate { get; set; }
    }
}
