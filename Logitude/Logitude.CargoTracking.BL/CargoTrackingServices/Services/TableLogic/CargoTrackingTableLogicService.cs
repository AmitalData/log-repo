
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableLogic
{
    public class CargoTrackingTableLogicService
    { 

        public static void SetTableLogic(DataRow TableRow, string TableName,int ConditionNumber)
        {
            switch (TableName)
            {
                case "CargoTrackingPorts":
                    {
                        CargoTrackingPortsLogicService.SetTableLogic(TableRow, ConditionNumber);
                        break;
                    }
                case "CargoTrackingShipments":
                    {
                        CargoTrackingShipmentsLogicService.SetTableLogic(TableRow, ConditionNumber);
                        break;
                    }
                case "CargoTrackingShipmentSearches":
                    {
                        CargoTrackingShipmentSearchesLogicService.SetTableLogic(TableRow, ConditionNumber);
                        break;
                    }

            }

       }
  
    }

}
