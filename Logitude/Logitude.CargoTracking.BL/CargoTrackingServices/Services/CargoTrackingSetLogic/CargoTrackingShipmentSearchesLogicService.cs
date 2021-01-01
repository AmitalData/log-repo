using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentSearchesLogicService
    {
        public static void SetTableLogic(DataRow tableRow,  int conditionNumber)
        {
            SetShipmentDate(tableRow);
            SetShipmentId(tableRow);
        }

        private static void SetShipmentDate(DataRow tableRow)
        {
            tableRow.SetField("ShipmentDate", tableRow["CreateDateTime"]);
 
        }
        private static void SetShipmentId(DataRow tableRow)
        {
            tableRow.SetField("ShipmentId", tableRow["Id"]);
        }
    }
}
