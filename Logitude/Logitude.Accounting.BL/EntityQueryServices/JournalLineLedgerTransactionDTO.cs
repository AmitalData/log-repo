using Logitude.Accounting.Data.EntityPOCOs;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public class JournalLineLedgerTransactionDTO
    {
        public JournalLine JournalLine { get; set; }
        public LedgerTransaction LedgerTransaction { get; set; }
    }
}