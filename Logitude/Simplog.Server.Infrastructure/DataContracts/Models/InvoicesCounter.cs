namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class InvoicesCounter
    {
        public double? MaxOpenAmount { get; set; }
        public double? MinOpenAmount { get; set; }
        public double? MaxTotalAmount { get; set; }
        public double? MinTotalAmount { get; set; }
    }
}
