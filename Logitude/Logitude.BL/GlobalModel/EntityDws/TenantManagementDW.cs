using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityDws
{
   public class TenantManagementDW
    {
        public string PackageCode { get; set; }
        public bool IsMultiPackage { get; set; }
        public int NumberOfUsers { get; set; }
        public DateTime? PaidUntilDate { get; set; }
        public int? ResellerCommission { get; set; }
        public int? FreeUsers { get; set; }
        public bool IsRecurring { get; set; }
        public double? LicensePrice { get; set; }
        public string PaymentCurrency { get; set; }
        public int TenantNumber { get; set; }
        public string PaymentChannel { get; set; }
        public string RecurringPeriod{ get; set; }
        public string Notes { get; set; }
        public string MainPackage { get; set; }
        public string CRMYN { get; set; }
        public string EAWBYN { get; set; }
        public int MainPackageNumberOfUsers { get; set; }
        public int CRMNumberOfUsers { get; set; }
        public int EAWBNumberOfUsers { get; set; }
        
    }
}
