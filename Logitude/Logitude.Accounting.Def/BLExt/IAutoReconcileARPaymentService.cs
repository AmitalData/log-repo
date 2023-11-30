using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.BLExt
{
    public interface IAutoReconcileServiceExt
    {
        void InitMust(GLAccountPM glAccountBillTO, JournalPM journalARPayment, List<AutoReconcileRecord> AutoReconcileRecordList, string accountingEntityCode);


        void InsertJournalReconcile();
    }

    public class AutoReconcileRecord
    {
        public string LedgerTransactionID { get; set; }
        public string JournalId { get; set; }
        //public string JournalLine { get; set; }
        public string AccountingEntityId { get; set; }
        public decimal LocalAmountToReconcile { get; set; }
        public decimal ForeignAmountToReconcile { get; set; }
        public string ForeignCurrencyIdReconcile { get; set; }
    }
}
