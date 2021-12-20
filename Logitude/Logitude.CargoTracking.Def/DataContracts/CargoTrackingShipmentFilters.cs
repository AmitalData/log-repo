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
            get
            {
                if (!string.IsNullOrWhiteSpace(CustomersIdsString))
                    return CustomersIdsString.Split(',').ToList();
                else
                    return new List<string>();
            }
        }

        public string MilestonesStatus { get; set; }
        public List<string> MilestonesCodes
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(MilestonesStatus))
                    return MilestonesStatus.Split(',').ToList();
                else
                    return new List<string>();
            }
        }

        public string TransportModeCodes { get; set; }
        public string DirectionCodes { get; set; }
        public string SortDescending { get; set; }
        public string SortFieldName { get; set; }
        public bool HasException { get; set; }
        public bool OrdersOnly { get; set; }
        public bool EstimatedArrivalOnly { get; set; }
        public bool OperationalClosedOnly { get; set; }

    }
}
