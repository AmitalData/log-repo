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

        public List<string> CustomersIds
        {
            get { return CustomersIdsString.Split(',').ToList(); }
        }

public string TransportModeCodes { get; set; }
        public string DirectionCodes { get; set; }
        public bool SortDescending { get; set; }
        public string SortFieldName { get; set; }

    }
}
