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
        public JournalPM GetJournalByAccountingEntityIdAndCode(string accountingEntityId, string accountingEntityCode, int tenant)
        {
            JournalQueryService query = new JournalQueryService(tenant);
            return query.GetByAccountingEntityIdAndAccountingEntityCode(accountingEntityId, accountingEntityCode, tenant);
        }
        public List<JournalPM> GetJournalsWithLinesByAccountingEntityIdAndCode(string accountingEntityId, string accountingEntityCode, int tenant)
        {
            JournalQueryService query = new JournalQueryService(tenant);
            return query.GetJournalsByAccountingEntityIdAndTypeCode(accountingEntityId, accountingEntityCode, tenant);
        }


        public JournalPM GetApprovedJournalByAccountingEntityId(string accountingEntityId, string accountingEntityCode, int tenant)
        {
            JournalQueryService query = new JournalQueryService(tenant);
            return query.GetApprovedJournalByAccountingEntityId(accountingEntityId, accountingEntityCode, tenant);
        }
        public JournalPM GetSingleWithLinesByEntityIdAndCode(string accountingEntityId, string accountingEntityCode, int tenant)
        {
            JournalQueryService query = new JournalQueryService(tenant);
            return query.GetSingleWithLinesByEntityIdAndCode(accountingEntityId, accountingEntityCode, tenant);
        }
    }
}

