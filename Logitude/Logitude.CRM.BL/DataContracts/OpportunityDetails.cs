using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.DataContracts
{
    public class OpportunityDetails
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
        public int? NumberOfUsers { get; set; }
        public double? AveragePrice { get; set; }
        public double? TotalPrice { get; set; }
        public int? TenantManagementNumberOfUsers { get; set; }
        public string Total { get; set; }
        public int Totalnet { get; internal set; }
        public string IsNewCustomer { get; internal set; }
    }
}
