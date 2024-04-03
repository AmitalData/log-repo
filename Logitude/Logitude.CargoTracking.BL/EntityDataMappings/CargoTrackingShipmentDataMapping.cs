
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
   
   public partial class CargoTrackingShipmentDataMapping: IMapping<CargoTrackingShipmentPM, CargoTrackingShipment>
   {

        public void CustomPMToPOCO(CargoTrackingShipmentPM entityPM, CargoTrackingShipment entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CargoTrackingShipmentPM entityPM, CargoTrackingShipment entityPOCO)
        {
            entityPM.ForwardingHouse = entityPOCO.ForwardingHouse;
            entityPM.ForwardingMaster = entityPOCO.ForwardingMaster;
            //throw new NotImplementedException();
        }
   }


}
   