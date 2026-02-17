using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IJournalUpdateServiceExt
    {
        void Update(JournalPM entityPM);
    }

    public class StornoOverrideM
    {
        public string AccountingEntityCode { get; set; }
        public string AccountingEntityReference { get; set; }
        public string AccountingEntityId { get; set; }
    }
}
