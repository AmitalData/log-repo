using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public class CargoTrackingShipmentComputedTableStructure
    {
        public string GetColumnsForCopy()
        {
            List<string> columnsForCopy = new List<string>();
            columnsForCopy.Add("Id");
            columnsForCopy.Add("FirstPickupATD");
            columnsForCopy.Add("FinalDeliveryATA");
            columnsForCopy.Add("FinalDeliveryETA");
            columnsForCopy.Add("Tenant");
            columnsForCopy.Add("AutomaticLastUpdateDate");
            columnsForCopy.Add("FinalDeliveryETD");
            columnsForCopy.Add("FinalDeliveryATD");

            return string.Join(",", columnsForCopy.ToArray());
        }


    }
}
