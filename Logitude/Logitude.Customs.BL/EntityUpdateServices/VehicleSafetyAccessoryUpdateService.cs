using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class VehicleSafetyAccessoryUpdateService
    {

       protected override void OnCreating(VehicleSafetyAccessoryPM entityPM, VehiclePM entityParentPM)
       {
           entityPM.VehicleId = entityParentPM.Id;

           entityParentPM.LastSaftyLineNumber += 1;
           entityPM.LineNumber = entityParentPM.LastSaftyLineNumber;

       }

    }
}
