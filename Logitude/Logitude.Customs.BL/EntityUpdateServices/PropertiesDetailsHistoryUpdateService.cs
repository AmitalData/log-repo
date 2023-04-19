using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial  class PropertiesDetailsHistoryUpdateService
    {
        protected override void OnCreating(PropertiesDetailsHistoryPM entityPM, CustomsItemPM entityParentPM)
        {
            entityPM.ID = IdCounter.GetNumber("Customs.PropertiesDetailsHistory",0 );


            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
