
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
   
   public partial class CargoTrackingWatermarkDataMapping: IMapping<CargoTrackingWatermarkPM, CargoTrackingWatermark>
   {

        public void CustomPMToPOCO(CargoTrackingWatermarkPM entityPM, CargoTrackingWatermark entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CargoTrackingWatermarkPM entityPM, CargoTrackingWatermark entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   