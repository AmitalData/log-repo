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
    }
}
