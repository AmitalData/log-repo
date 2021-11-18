
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic;
using Logitude.CargoTracking.Data.EntityLists;
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
        const int ShipmentOrderTableCondition = 3;

        public static void SetTableLogic(DataRow tableRow, string tableName, int conditionNumber, List<CargoTrackingMilestoneList> milestoneList)
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
                        SetCargoTrackingShipmentsLogic(conditionNumber, tableRow, milestoneList);
                        break;
                    }
                case "CargoTrackingShipmentSearches":
                    {
                        CargoTrackingShipmentSearchesLogicService.SetTableLogic(tableRow, conditionNumber);
                        break;
                    }

            }

       }

        private static void SetCargoTrackingShipmentsLogic(int conditionNumber, DataRow tableRow, List<CargoTrackingMilestoneList> milestoneList)
        {
            if (conditionNumber == ShipmentOrderTableCondition)
                CargoTrackingShipmentsOrderLogicService.SetTableLogic(tableRow, milestoneList);
            else
                CargoTrackingShipmentsLogicService.SetTableLogic(tableRow, conditionNumber, milestoneList);
        }
    }

}
