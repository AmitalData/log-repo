using Logitude.CargoTracking.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTrackingShipmentResources
    {
        public string CustomsDeclarationNumber { get; set; }
        public CargoTrackingShipmentResources Child { get; set; }
        public CargoTrackingShipment CargoTrackingShipment { get; set; }

        
    }
}
