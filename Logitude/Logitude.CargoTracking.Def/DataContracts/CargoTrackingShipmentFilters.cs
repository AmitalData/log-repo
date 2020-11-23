using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.Def.DataContracts
{
    public class CargoTrackingShipmentFilters
    {
        public int Tenant { get; set; }
        public string SearchText { get; set; }
        public string CustomersIdsString { get; set; }
        public List<string> CustomersIds { get; set; }
        public string TransportModeCodes { get; set; }
        public string DirectionCodes { get; set; }
        //public bool GetAirShipments { get; set; }
        //public bool GetLandShipments { get; set; }
        //public bool GetOceanShipments { get; set; }
        //public bool GetImportShipments { get; set; }
        //public bool GetExportShipments { get; set; }
    }
}
