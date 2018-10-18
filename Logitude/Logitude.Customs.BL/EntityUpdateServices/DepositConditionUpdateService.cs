using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class DepositConditionUpdateService
    {

       protected override void OnCreating(DepositConditionPM entityPM, DepositPM entityParentPM)
       {
           //entityPM.DepositId = entityParentPM.Id;

           if (entityParentPM == null)
           {
               return;
           }
           entityPM.DepositId = entityParentPM.Id;
           entityPM.Tenant = entityPM.Tenant;

           base.OnCreating(entityPM, entityParentPM);

       }
    }
}
