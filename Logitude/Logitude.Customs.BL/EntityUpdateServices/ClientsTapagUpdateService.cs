using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientsTapagUpdateService : EntityUpdateService<ClientsTapag, ClientsTapagPM, ClientPM>
    {

        protected override void OnCreating(ClientsTapagPM entityPM, ClientPM entityParentPM)
        {
            entityPM.ClientId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.Id = IdCounter.GetNumber("ClientsTapag", entityPM.Tenant);
        }

    }
}
