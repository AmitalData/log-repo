
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.ShipmentOrderLib.Data.EntityPOCOs;
using Logitude.ShipmentOrderLib.BL.EntityPMs; 
using Logitude.ShipmentOrderLib.Data;

namespace Logitude.ShipmentOrderLib.BL.EntityDataMappings
{
   
   public partial class ShipmentOrderDataMapping: IMapping<ShipmentOrderPM, ShipmentOrder>
   {

        public void CustomPMToPOCO(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   