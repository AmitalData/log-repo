 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingUpdatedAfterFinishMapping
    {
         
        public static void UpdatedAfterFinishMapping(DataRow TableRow, string TableName)
        {
            if (TableName== "CargoTrackingShipments")
            {
                if (CargoTrackingTableLogicAfterUpdating.ForwardingShipments.Count() > 0 )
                {
                    string TargetKey = null;
                    foreach (KeyValuePair<string, string> entry in CargoTrackingTableLogicAfterUpdating.ForwardingShipments)
                    {
                        if (entry.Value.Equals(TableRow["Id"]))
                        {
                            TableRow.SetField("ForwardingShipmentHeaderId", entry.Key);
                            TargetKey = entry.Key;
                        }
                    }
                    if (!string.IsNullOrEmpty(TargetKey))
                    {
                        CargoTrackingTableLogicAfterUpdating.ForwardingShipments.Remove(TargetKey);
                    }
                }

            }

        }

    }

}
