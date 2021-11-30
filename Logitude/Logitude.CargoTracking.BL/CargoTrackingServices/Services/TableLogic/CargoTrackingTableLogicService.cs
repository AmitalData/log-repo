
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
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

        public static void SetTableLogic(SetTableLogicArgs args)
        {
            switch (args.TableName)
            {
                case "CargoTrackingPorts":
                    {
                        CargoTrackingPortsLogicService.SetTableLogic(args.TableRow, args.ConditionNumber);
                        break;
                    }
                case "CargoTrackingShipments":
                    {
                        SetCargoTrackingShipmentsLogic(args);
                        break;
                    }
                case "CargoTrackingShipmentSearches":
                    {
                        CargoTrackingShipmentSearchesLogicService.SetTableLogic(args.TableRow, args.ConditionNumber);
                        break;
                    }

            }

       }

        private static void SetCargoTrackingShipmentsLogic(SetTableLogicArgs args)
        {
            if (args.ConditionNumber == ShipmentOrderTableCondition)
                CargoTrackingShipmentsOrderLogicService.SetTableLogic(args);
            else
                CargoTrackingShipmentsLogicService.SetTableLogic(args);
        }
    }

}
