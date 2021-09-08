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
        public string InvoiceNumber { get; set; }
        public DateTime? InterestCalculationDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<InterestReportLinesByDateProvider> InterestReportLinesByDateList { get; set; }
        public decimal? CreditAllotmentPercentage { get; set; }
        public decimal? CalCreditAllotmentCommission { get; set; }
        public decimal? CalculatedPostponedChequesCommision { get; set; }
        public decimal? AllotmentCommession { get; set; }
        public string AllotmentCalculation { get; set; }
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

}