using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    public class JournalLite
    {
        public string JournalId { get; set; }
        public bool IsLedgerCreated { get; set; }
    }
}
