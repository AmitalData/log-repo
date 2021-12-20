using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.Def.DataContracts
{
    public class CargoTrackingShipmentSearchInput
    {
        public int Tenant { get; set; }
        public string SearchText { get; set; }
        public List<string> CustomersIds { get; set; }
        public List<string> MilestonesCodes { get; set; }
        public List<string> TransportModeCodes { get; set; }
        public List<string> DirectionCodes { get; set; }
        public string SortType { get; set; }
        public string SortFieldName { get; set; }
        public bool HasException { get; set; }
        public bool OrdersOnly { get; set; }
        public bool EstimatedArrivalOnly { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool OperationalClosedOnly { get; set; }

    }
}
