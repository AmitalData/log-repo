
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

        public static void SetTableLogic(DataRow tableRow, string tableName,int conditionNumber)
        {
            switch (tableName)
            {
                case "CargoTrackingPorts":
                    {
                        CargoTrackingPortsLogicService.SetTableLogic(tableRow, conditionNumber);
                        break;
                    }
                case "CargoTrackingShipments":
                    {
                        CargoTrackingShipmentsLogicService.SetTableLogic(tableRow, conditionNumber);
                        break;
                    }
                case "CargoTrackingShipmentSearches":
                    {
                        CargoTrackingShipmentSearchesLogicService.SetTableLogic(tableRow, conditionNumber);
                        break;
                    }

            }

       }
  
    }

}
