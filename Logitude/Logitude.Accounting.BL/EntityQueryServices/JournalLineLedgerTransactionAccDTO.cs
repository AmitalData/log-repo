using Logitude.Accounting.Data.EntityPOCOs;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public class JournalLineLedgerTransactionAccDTO
    {
        public JournalLine JournalLine { get; set; }
        public LedgerTransaction LedgerTransaction { get; set; }
        public string AccId { get; set; }
    }
}
