
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs; 
using Logitude.CargoTracking.Data;

namespace Logitude.CargoTracking.BL.EntityDataMappings
{
   
   public partial class CargoReferencesSyncQueueDataMapping: IMapping<CargoReferencesSyncQueuePM, CargoReferencesSyncQueue>
   {

        public void CustomPMToPOCO(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueue entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueue entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   