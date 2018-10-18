using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class RequiredGuaranteeTypeUpdateService
    {

       protected override void OnCreating(RequiredGuaranteeTypePM entityPM, GuaranteePM entityParentPM)
       {
           entityPM.Id = IdCounter.GetNumber("Customs.RequiredGuaranteeType", entityPM.Tenant);
           entityPM.GuaranteeId = entityParentPM.Id;
           entityPM.Tenant = entityParentPM.Tenant;
       }


    }
}
