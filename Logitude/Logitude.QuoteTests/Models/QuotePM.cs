using System.Collections.Generic;

namespace Logitude.QuoteTests.Models
{
    public class QuotePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string BusinessUnitId { get; set; }
        public string StageId { get; set; }
        public string QuoteNumber { get; set; }
        public string CustomerId { get; set; }
        public string BranchId { get; set; }
        public string ConcurrencyGUID { get; set; }
        public string DepartmentId { get; set; }
        public string QuoteCustomerTypeCode { get; set; }
        public double? ExchangeRate { get; set; }
        public string SaleCurrencyId { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string DirectionId { get; set; }
        public string QuoteTypeCode { get; set; }
        public string TransportModeId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public int? NumberOfPackages { get; set; }
        public List<QuoteChargePM> QuoteCharges { get; set; }
        public List<QuotePackagePM> QuotePackages { get; set; }
    }
}
