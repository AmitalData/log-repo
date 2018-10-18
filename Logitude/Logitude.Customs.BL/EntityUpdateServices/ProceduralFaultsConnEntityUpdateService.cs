using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class ProceduralFaultsConnEntityUpdateService
    {

       protected override void OnCreating(ProceduralFaultsConnEntityPM entityPM, ProceduralFaultPM entityParentPM)
       {
           entityPM.Id = IdCounter.GetNumber("Customs.ProceduralFaultsConnEntity", entityPM.Tenant);
           entityPM.ProceduralFaultId = entityParentPM.Id;
           base.OnCreating(entityPM, entityParentPM);
       }

    }
}
