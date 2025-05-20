using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class MagayaCommunicationLogQueryService : EntityQueryService<MagayaCommunicationLog, MagayaCommunicationLogKeys, MagayaCommunicationLogPM, object, MagayaCommunicationLogKeys>
    {

        public List<MagayaCommunicationLog> GetMulti(MagayaCommunicationLogKeys entityKeys)
        {

            throw new NotImplementedException();
        }
        public MagayaCommunicationLogPM GetByCommunicationId(string communicationId, int tenant)
        {
            MagayaCommunicationLog MyPoco = repository.GetByCommunicationId(tenant, communicationId);
            return this.GetEntityPM(MyPoco);
        }

          
    }
}
