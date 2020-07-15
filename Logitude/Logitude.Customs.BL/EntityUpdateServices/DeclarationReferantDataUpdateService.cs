using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationReferantDataUpdateService
    {

        protected override void OnCreating(DeclarationReferantDataPM entityPM, EntityPM entityParentPM)
        {
            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void OnUpdating(DeclarationReferantDataPM entityPM)
        {
            base.OnUpdating(entityPM);
        }
    }
}
