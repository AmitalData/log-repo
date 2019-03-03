using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class GatepassRequestUpdateService : EntityUpdateService<GatepassRequest, GatepassRequestPM, EntityPM>
    {
        protected override void OnCreating(GatepassRequestPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.GatepassNumber == 0)
            {
                entityPM.GatepassNumber = CodeCounter.GetNumber("Customs.GatepassRequest", entityPM.Tenant);
            }
            if(entityPM.CustomsUpdateDateTime == null)
            {
                entityPM.CustomsUpdateDateTime = DateTime.Now;
            }

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
