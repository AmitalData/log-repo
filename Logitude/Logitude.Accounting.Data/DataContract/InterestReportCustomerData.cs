using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.DataContract
{
    public class InterestReportCustomerData
    {
        public string CustomerId { get; set; }
        public string GLAccountId { get; set; }
        public int Tenant { get; set; }
        public bool? ActiveForInterest { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
        public decimal? InterestCreditLimit { get; set; }
        public int? MinimumInterestInvoiceBilling { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
    }
}
