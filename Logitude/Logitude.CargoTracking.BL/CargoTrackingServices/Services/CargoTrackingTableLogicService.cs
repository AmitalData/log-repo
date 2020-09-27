
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingTableLogicService
    {
        public static Dictionary<string, string> ForwardingShipments = new Dictionary<string, string>();


        public static void SetTableLogic(DataRow TableRow, string TableName,int ConditionNumber)
        {
            if (TableName == "CargoTrackingPorts")
            {
                CargoTrackingPortsLogicService.SetTableLogic(TableRow, ConditionNumber);

            }

            if (TableName == "CargoTrackingShipments")
            {
                CargoTrackingShipmentsLogicService.SetTableLogic(TableRow, ConditionNumber);

            }

            if (TableName == "CargoTrackingShipmentSearches")
            {
                CargoTrackingShipmentSearchesLogicService.SetTableLogic(TableRow,ConditionNumber);
                  
            }

        }
  
    }

}
