using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationErrorMappingUpdateService
    {
        protected override void OnCreating(DeclarationErrorMappingPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.DeclarationErrorMapping", 0);
            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
