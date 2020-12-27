using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public static class CargoTrackingTableList
    {
        public static List<CargoTrackingTable> FillCargoTableList()
        {
            List<CargoTrackingTable> CargoTableLists = new List<CargoTrackingTable>();

            CargoTableLists.Add(new CargoTrackingTable("CargoTrackingPort")
            {
                DBTableName = "Ports",
                ConditionKey = "Id",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTrackingTable("CargoTrackingCard")
            {
                DBTableName = "Cards",
                ConditionKey = "Id",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTrackingTable("CargoTrackingTransportMode")
            {
                DBTableName = "TransportModes",
                ConditionKey = "Id",
                ConditionsNumber = 1,
                IsClosedTable = true,
            });

            CargoTableLists.Add(new CargoTrackingTable("CargoTrackingCountry")
            {
                DBTableName = "Countries",
                ConditionKey = "Id",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTrackingTable("CargoTrackingShipmentMaster")
            {
                DBTableName = "ShipmentMasterDatas",
                ConditionKey = "Id",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTrackingTable("CargoTrackingShipmentComputed")
            {
                DBTableName = "ShipmentComputedFields",
                ConditionKey = "Id",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTrackingTable("CargoTrackingShipment", "CargoTrackingShipmentSearch")
            {
                DBTableName = "Shipments",
                ConditionKey = "EntityId",
                InnerConditionKey = "ShipmentId",
                ConditionsNumber = 2,
            });

            return CargoTableLists;

        }
    }
}
