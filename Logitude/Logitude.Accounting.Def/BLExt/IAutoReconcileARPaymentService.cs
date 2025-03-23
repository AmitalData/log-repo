using Logitude.Accounting.Def.EntityPMs;
using System.Collections.Generic;

namespace Logitude.Accounting.Def.BLExt
{
    public interface IAutoReconcileServiceExt
    {
        void InitMust(GLAccountPM glAccountBillTO, JournalPM journalARPayment, List<AutoReconcileRecord> AutoReconcileRecordList, string accountingEntityCode,string paymentCurrencyId);


        void InsertJournalReconcile();
    }

    public class AutoReconcileRecord
    {
        public string LedgerTransactionID { get; set; }
        public string JournalId { get; set; }
        public string AccountingEntityId { get; set; }
        public decimal LocalAmountToReconcile { get; set; }
        public decimal ForeignAmountToReconcile { get; set; }
        public string ForeignCurrencyIdReconcile { get; set; }
    }
}
