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
    public partial class ClientIndicationUpdateService : EntityUpdateService<ClientIndication, ClientIndicationPM , ClientPM>
    {
        protected override void OnCreating(ClientIndicationPM entityPM, ClientPM entityParentPM)
        {
            if (entityPM == null)
                return;

            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.IndicationId = IdCounter.GetNumber("Customs.ClientIndication", entityPM.Tenant);
            entityPM.CreateDate = DateTime.Now;
          
        }


    }
}
