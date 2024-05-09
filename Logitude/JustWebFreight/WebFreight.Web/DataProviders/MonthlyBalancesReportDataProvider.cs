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
            MonthlyBalancesLine = new List<MonthlyBalancesLine>();
        }
          
         public List<MonthlyBalancesLine> MonthlyBalancesLine { get; set; }
    }


    public class MonthlyBalancesLine
    {
        
        public int QuantityForJanuary {  get; set; }
        public int QuantityForFebruary { get; set; }
        public int QuantityForMarch { get; set; }
        public int QuantityForApril { get; set; }
        public int QuantityForMay { get; set; }
        public int QuantityForJune { get; set; }
        public int QuantityForJuly { get; set; }
        public int QuantityForAugust { get; set; }
        public int QuantityForSeptember { get; set; }
        public int QuantityForOctober { get; set; }
        public int QuantityForNovember { get; set; }
        public int QuantityForDecember { get; set; }
        public int OpenBalance { get; set; }
        public string GLAcountLocalName { get; set; }
        public string GLAcountNumber { get; set; }
        public int TotalReport { get; set; }




    }


}