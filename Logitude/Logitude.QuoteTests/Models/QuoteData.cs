using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.QuoteTests.Models
{
    public static class QuoteData
    {
        public static string Id { get; set; }
        public static int Tenant { get; set; }
        public static string BusinessUnitId { get; set; }
        public static string StageId { get; set; }
        public static string QuoteNumber { get; set; }
        public static string CustomerId { get; set; }
        public static string BranchId { get; set; }
        public static string ConcurrencyGUID { get; set; }
        public static string DepartmentId { get; set; }
        public static string QuoteCustomerTypeCode { get; set; }
        public static double? ExchangeRate { get; set; }
        public static string SaleCurrencyId { get; set; }
        public static string CreatedByUserId { get; set; }
        public static string UpdatedByUserId { get; set; }
        public static string DirectionId { get; set; }
        public static string QuoteTypeCode { get; set; }
        public static string TransportModeId { get; set; }
        public static string FromPortId { get; set; }
        public static string ToPortId { get; set; }
        public static int? NumberOfPackages { get; set; }
        public static List<QuoteChargePM> QuoteCharges { get; set; }
        public static List<QuotePackagePM> QuotePackages { get; set; }
    }
}
