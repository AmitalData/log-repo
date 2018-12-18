using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
   public class OpenFormatReportData
    {

        public List<B100Data> B100DataList;
    }

    public class B100Data
    {
        public int Counter { get; set; }
        public string JournalNumber { get; set; }
        public int JournalLineNumber { get; set; }
        public string AccountingEntityReference { get; set; }
        public string AccountingEntityCode { get; set; }
        public string Reference2 { get; set; }
        public string Notes { get; set; }
        public DateTime AccountingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public string GLAccountDisplayNumber { get; set; }
        public decimal? LocalAmountDebit { get; set; }
        public string CurrencyId { get; set; }
        public decimal? LocalAmountCredit { get; set; }
        public decimal? ForeignAmountDebit { get; set; }
        public decimal? ForeignAmountCredit { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUser { get; set; }




    }

}
