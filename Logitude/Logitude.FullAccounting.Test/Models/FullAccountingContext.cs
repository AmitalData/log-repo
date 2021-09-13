using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Models
{
    public class FullAccountingContext
    {
        public List<JournalLinePM> Journallines { get; internal set; }
        public JournalPM ApprovedJournal { get; internal set; }
        public JournalPM AddedApprovedJournal { get; internal set; }
        public GLAccountPM GLAccount { get; internal set; }
        public GLAccountPM UpdatedGLAccount { get; internal set; }
    }
}
