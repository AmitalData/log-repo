using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.DataContracts
{
    public class OpportunityCRMDetails
    {
        public string CustomerStatusCode { get; set; }
        public string ResellerId { get; set; }
        public string OpportunityTypeId { get; set; }
        public string ClientId { get; set; }
        public string TenantNumber { get; set; }
        public string ClientName { get; set; }
        public string Reseller { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public int? ResellerCommission { get; set; }
        public decimal? TenantManagementAveragePrice { get; set; }
        public decimal? TenantManagementTotalPrice { get; set; }
        public int? TenantManagementNumberOfUsers { get; set; }
        public int? NumberOfUsers { get; set; }
        public decimal Total { get; set; }
        public string Field4 { get; set; }
        public decimal? TotalNet { get; set; }
        public int? IsNewCustomer { get; set; }
        public string OpportunityTypeCode { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CreateDate { get; set; }
        public bool InActive { get; set; }
        public DateTime ActualClosingDate { get; set; }
        public string PaymentChannelCode { get; set; }
    }
}
