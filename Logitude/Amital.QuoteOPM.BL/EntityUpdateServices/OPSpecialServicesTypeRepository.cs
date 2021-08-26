using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    public partial class OPSpecialServicesTypeUpdateService : EntityUpdateService<OPSpecialServicesType, OPSpecialServicesTypePM, EntityPM>
    {
        protected override void OnCreating(OPSpecialServicesTypePM entityPM, EntityPM entityParentPM)
        {
            EntityPM.Id = IdCounter.GetNumber("OPSpecialServicesType", entityPM.Tenant).ToString();
            base.OnCreating(entityPM, entityParentPM);
        }

    }
}
