using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class VehicleOwnerUpdateService
    {

       protected override void OnCreating(VehicleOwnerPM entityPM, VehiclePM entityParentPM)
       {
           entityPM.VehicleId = entityParentPM.Id;

           entityParentPM.LastOwnerLineNumber += 1;
           entityPM.LineNumber = entityParentPM.LastOwnerLineNumber;

       }
    }
}
