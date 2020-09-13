using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentSearchesLogicService
    {
        public static void SetTableLogic(DataRow TableRow,  int ConditionNumber)
        {
            SetShipmentDate(TableRow, ConditionNumber);
        }

        private static void SetShipmentDate(DataRow TableRow, int ConditionNumber)
        {
            TableRow.SetField("ShipmentDate", TableRow["CreateDateTime"]);
            TableRow.SetField("ShipmentId", TableRow["Id"]);
            TableRow.SetField("IsPublic", true);

        }
    }
}
