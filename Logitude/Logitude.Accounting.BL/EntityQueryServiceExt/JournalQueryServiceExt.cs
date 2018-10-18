using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
    public class JournalQueryServiceExt : IJournalQueryServiceExt
    {
        public JournalQueryServiceExt()
        {

        }

        public JournalPM GetJournalIdByAccountingEntityId(string entityId, int tenant)
        {
            JournalQueryService query = new JournalQueryService(tenant);
            return query.GetByAccountingEntityId(entityId, tenant);
        }
    }
}
