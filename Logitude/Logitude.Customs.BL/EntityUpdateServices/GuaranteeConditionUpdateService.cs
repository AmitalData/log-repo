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
  public partial  class GuaranteeConditionUpdateService
    {

      protected override void OnCreating(GuaranteeConditionPM entityPM, GuaranteePM entityParentPM)
      {
          entityPM.Id = IdCounter.GetNumber("Customs.GuaranteeCondition", entityPM.Tenant);
          entityPM.GuaranteeId = entityParentPM.Id;
          entityPM.Tenant = entityParentPM.Tenant;
      }

    }
}
