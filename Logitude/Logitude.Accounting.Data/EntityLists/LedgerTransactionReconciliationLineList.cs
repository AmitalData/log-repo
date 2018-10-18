using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.EntityLists
{
    public class LedgerTransactionReconciliationLineList
    {
        public LedgerTransactionList LedgerTransactionPart
        {
            get { return ledgerTransaction; }
            set
            {
                ledgerTransaction = value;
            }
        }
        public ReconciliationLineList ReconciliationLinePart
        {
            get { return reconciliationLine; }
            set
            {
                reconciliationLine = value;
            }
        }

        private LedgerTransactionList ledgerTransaction;
        private ReconciliationLineList reconciliationLine;

        public LedgerTransactionReconciliationLineList(LedgerTransactionList ledgerTransaction, ReconciliationLineList reconciliationLine)
        {
            this.ledgerTransaction = ledgerTransaction;
            this.reconciliationLine = reconciliationLine;
        }
    }
}
