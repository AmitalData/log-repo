using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ExpectedIncomeDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<CRMCustomer> RecordList { get; set; }
        public List<ErrorItemClass> ErrorsList { get; set; }
    }

    public class CRMCustomer
    {
        [Key]
        public int Id { get; set; }
        public int TenantNumber { get; set; }
        public string CustomerName { get; set; }
        public string ResellerName { get; set; }
        public string SalesmanName { get; set; }
        public string CountryName { get; set; }
        public int? LicensedUsersNumber { get; set; }
        public int? FreeUsersNumber { get; set; }
        public string RecurringPeriodCode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentChannelName { get; set; }
        public string SalesNotes { get; set; }

        public string CurrencyCode { get; set; }
        public double? Price { get; set; }
        public double? Total { get; set; }
        public DateTime? PaidUntilDate { get; set; }
        public double? TotalInUSD { get; set; }
        public double? TotalInEUR { get; set; }
        public double? GrandTotalInUSD { get; set; }
        public double? GrandTotalInEUR { get; set; }

        public string Package { get; set; }
    }

    public class ErrorItemClass
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
    }
}