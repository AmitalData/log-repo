using Logitude.CargoTracking.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTrackingShipmentContext
    {
        public string CustomsDeclarationNumber { get; set; }
        public CargoTrackingShipmentContext Chiled { get; set; }
        public CargoTrackingShipment CargoTrackingShipment { get; set; }

        
    }
}
