using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using System.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientsPoaUpdateService : EntityUpdateService<ClientsPoa, ClientsPoaPM, ClientPM>
    {
        protected override void OnCreating(ClientsPoaPM entityPM, ClientPM entityParentPM)
        {
            if (entityPM == null)
                return;

            entityPM.Id = IdCounter.GetNumber("Customs.ClientsPoa", entityPM.Tenant);
            entityPM.Tenant = entityParentPM.Tenant;


        }
       
    }
}
