using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class LogisticActionRequestQueryService
    {
        public LogisticActionRequestPM GetByCargoKey(string key1, string key2, string key3, int type)
        {
            LogisticActionRequest logisticActionRequest = new LogisticActionRequestRepository(Tenant).GetByCargoKey(key1, key2, key3, type);

            return logisticActionRequest == null ? null : GetEntityPM(logisticActionRequest);
        }
    }
}
