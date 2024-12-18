using Logitude.Accounting.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class MonthlyBalancesReportDataProvider : BaseDataProvider
    {
        public MonthlyBalancesReportDataProvider()
        {
            ChartOfAccountLine = new List<ChartOfAccountLine>();
        }
           
         public List<ChartOfAccountLine> ChartOfAccountLine { get; set; }
    }


    public class ChartOfAccountLine
    {
        
        public decimal QuantityForJanuary {  get; set; }
        public decimal QuantityForFebruary { get; set; }
        public decimal QuantityForMarch { get; set; }
        public decimal QuantityForApril { get; set; }
        public decimal QuantityForMay { get; set; }
        public decimal QuantityForJune { get; set; }
        public decimal QuantityForJuly { get; set; }
        public decimal QuantityForAugust { get; set; }
        public decimal QuantityForSeptember { get; set; }
        public decimal QuantityForOctober { get; set; }
        public decimal QuantityForNovember { get; set; }
        public decimal QuantityForDecember { get; set; }
        public decimal OpenBalance { get; set; }
        public string GLAcountLocalName { get; set; }
        public string GLAcountNumber { get; set; }
        public decimal TotalReport { get; set; }
        public string GLAcountEnglishName { get; set; }

        public List<MonthlyBalancesLine> MonthlyBalancesLine { get; set; }


    }
    public  class MonthlyBalancesLine
    {
        public decimal QuantityForJanuary { get; set; }
        public decimal QuantityForFebruary { get; set; }
        public decimal QuantityForMarch { get; set; }
        public decimal QuantityForApril { get; set; }
        public decimal QuantityForMay { get; set; }
        public decimal QuantityForJune { get; set; }
        public decimal QuantityForJuly { get; set; }
        public decimal QuantityForAugust { get; set; }
        public decimal QuantityForSeptember { get; set; }
        public decimal QuantityForOctober { get; set; }
        public decimal QuantityForNovember { get; set; }
        public decimal QuantityForDecember { get; set; }
        public decimal OpenBalance { get; set; }
        public string GLAcountLocalName { get; set; }
        public string GLAcountNumber { get; set; }
        public string ChartOfAccount { get; set; }
        public string GLAcountEnglishName { get; set; }

        public decimal TotalReport { get; set; }
    }

}