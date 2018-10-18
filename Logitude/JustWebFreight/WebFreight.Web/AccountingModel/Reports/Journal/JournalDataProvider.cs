using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.AccountingModel.Reports.Journal
{
    public class JournalDataProvider
    {

        public string JournalNumber { get; set; }

        public string AccountingEntityCode { get; set; }
        //public AccountingEntity AccountingEntity { get; set; }

        public string AccountingEntityReference { get; set; }

        public DateTime AccountingDate { get; set; }

        //public User PrintedByUser { get; set; }
        public string PrintedByUserName { get; set; }

        public string TenantCurrency { get; set; }

        public List<JournalLine> JournalLines { get; set; }

    }

    public class JournalLine
    {
        public int Line { get; set; }

        public string JournalActionCode { get; set; }
        public string JournalActionName { get; set; }
        public string JournalActionTypeCode { get; set; }

        public DateTime DocumentDate { get; set; }

        public DateTime DueDate { get; set; }

        public string CreditAccountNumber { get; set; }
        public string CreditAccountName { get; set; }

        public string DebitAccountNumber { get; set; }
        public string DebitAccountName { get; set; }


        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        public decimal LocalAmount { get; set; }
        public decimal ForeignAmount { get; set; }

        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
    }
}
