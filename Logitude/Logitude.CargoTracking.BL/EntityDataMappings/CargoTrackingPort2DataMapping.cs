
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
   
   public partial class CargoTrackingPort2DataMapping: IMapping<CargoTrackingPort2PM, CargoTrackingPort2>
   {

        public void CustomPMToPOCO(CargoTrackingPort2PM entityPM, CargoTrackingPort2 entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CargoTrackingPort2PM entityPM, CargoTrackingPort2 entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   