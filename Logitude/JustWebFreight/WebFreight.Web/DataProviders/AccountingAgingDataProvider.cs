using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class AccountingAgingDataProvider : BaseDataProvider
    {
        public AccountingAgingDataProvider()
        {
            AgingPeriods = new List<AgingPeriod>();
        }
        public string CustomerFilterValue { get; set; }
        public DateTime? Month { get; set; }
        public string PrintedByUser { get; set; }

        public List<AgingPeriod> AgingPeriods { get; set; }
    }

    public class AgingPeriod
    {
        public string PeriodName { get; set; }
        public string CreditOrDebit { get; set; } // contains credit/debit labels 
        public decimal Total { get; set; } // contains credit/debit total 
        public decimal GrandTotal { get; set; } // used to calculate credit total and debit total from two records
        public int OrderIndex { get; set; }
        public List<AgingPeriodTotal> Totals { get; set; }


        public string AccountEnglishName { get; set; }
        public string AccountLocalName { get; set; }
        public string AccountName { get; set; }

    }

    public class AgingPeriodTotal
    {
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
    }
}