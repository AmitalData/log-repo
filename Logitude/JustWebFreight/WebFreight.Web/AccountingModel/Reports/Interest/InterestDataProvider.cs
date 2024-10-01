using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.App_Code.AngularJS_App_Code.Generated;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.AccountingModel.Reports.Interest
{
    public class InterestDataProvider: BaseDataProvider
    {
        public InterestDataProvider()
        {
            InterestReportLinesByDateList = new List<InterestReportLinesByDateProvider>();
        }
        public decimal? OpenBalance { get; set; }
        public string CustomerName { get; set; }
        public string GLAccountDisplayNumber { get; set; }

        public string InvoiceNumber { get; set; }
        public DateTime? InterestCalculationDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<InterestReportLinesByDateProvider> InterestReportLinesByDateList { get; set; }
        public decimal? CreditAllotmentPercentage { get; set; }
        public decimal? CalCreditAllotmentCommission { get; set; }
        public decimal? CalculatedPostponedChequesCommision { get; set; }
        public decimal? AllotmentCommession { get; set; }
        public string AllotmentCalculation { get; set; }
        public List <FutureInterestTransactionProvider> FutureInterestTransactions { get; set; }
    }

    public class InterestReportLinesByDateProvider
    {
        public InterestReportLinesByDateProvider()
        {
            InterestTransactionList = new List<InterestTransactionProvider>();
        }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal AccumulatedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalInterestDays { get; set; }
        public decimal StandardInterestPercentage { get; set; }
        public decimal ExceptionalInterestPercentage { get; set; }
        public decimal CreditInterestPercentage { get; set; }
        public string CalculationDetails { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalLocalAmount { get; set; }
        public List<InterestTransactionProvider> InterestTransactionList { get; set; }
        public List<InterestTransactionProvider> GroupedInterestTransactionList { get; set; }

    }
    public class InterestTransactionProvider
    {
        public string EntityType { get; set; }
        public string EntityNumber { get; set; }
        public decimal LocalAmount { get; set; }
        public DateTime? InterestValueDate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? ForeignAmount { get; set; }

     }

    public class FutureInterestTransactionProvider 
    {
       // public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string SearchFields { get; set; }
        public string InterestEntityTypeCode { get; set; }
        public int OriginalEntityLineNumber { get; set; }
        public decimal LocalAmount { get; set; }
        public decimal? ForeignAmount { get; set; }
        public DateTime? InterestValueDate { get; set; }
        public bool IsClosed { get; set; }
        public string CurrencyCode { get; set; }
        public string EntityNumber { get; set; }
        public string JournalNumber { get; set; }
        public string InterestEntityType { get; set; }
        public string EntityType { get; set; }
        public string AccountEntityCode { get; set; }
        public bool IsCancelled { get; set; }
        public string InterestReportNumber { get; set; }
        public string Source { get; set; }
        public string SourceTypeCode { get; set; }
        public string SourceType { get; set; }
        public string AccountingEntityCode { get; set; }
        public DateTime AccountingDate { get; set; }
        public string Notes { get; set; }
    }

}