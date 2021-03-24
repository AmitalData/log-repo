using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ExternalReconciliationLinesReportDataProvider : BaseDataProvider
    {
        public ExternalReconciliationLinesReportDataProvider()
        {
            BankDetails = new List<BankDetails>();
        }
        public string SortBy { get; set; }
        public string Type { get; set; }
        public DateTime? RefDateFrom { get; set; }
        public DateTime? RefDateTo{ get; set; }
        public string BankAccountId { get; set; }
        public string IsExternalReconciled { get; set; }
        public bool IncludesTransferGlaccount { get; set; }
        public int? ExternalReconciliationNumber { get; set; }
      
        public List<BankDetails> BankDetails { get; set; }
      
    }

    public class ExternalReconciliationPeriod
    {
        public string EnglishType { get; set; }
        public string LocalBoolean { get; set; }
        public string EnglishBoolean { get; set; }
        public string Type { get; set; }
        public string LocalType { get; set; }
        public string Number { get; set; }
        public string EntitySource { get; set; }
        public string EntityType { get; set; }
        public bool IsRecomncile { get; set; }
        public decimal? Amount { get; set; }
        public int? ReconcileNumber { get; set; }
        public string Note { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public DateTime? ReferenceDate {get;set;}
        public string BankAccountId { get; set; }
        public bool IsDuplicated { get; set; }
        public string ExternalPageLineId { get; set; }
        public string GLAccountId { get; set; }
    }

    public class BankDetails
    {
        public string BankAccountEnglishName { get; set; }

        public string BankAccountLocalName { get; set; }

        public string BankAccountCode { get; set; }

        public string BankAccountId { get; set; }

        public List<ExternalReconciliationPeriod> ExternalReconciliationPeriods { get; set; }

        public decimal? TotalClosed { get; set; }

        public decimal? TotalOpen { get; set; }

        public decimal? GlaccountTotalOpened { get; set; }

        public decimal? GlaccountTotalClosed { get; set; }

        public decimal? TransferTotalOpened { get; set; }

        public decimal? TransferTotalClosed { get; set; }

        public decimal? BankTotalOpened { get; set; }

        public decimal? BankTotalClosed { get; set; }

        public decimal? TotalInLocalCurrency { get; set; }

    }

}