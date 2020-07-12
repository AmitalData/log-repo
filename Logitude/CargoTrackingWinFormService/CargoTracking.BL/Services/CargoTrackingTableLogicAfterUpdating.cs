using CargoTrackingWinFormService.CargoTracking.BL.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrackingWinFormService.CargoTracking.BL.Services
{
    public class CargoTrackingTableLogicAfterUpdating
    {
        public static Dictionary<string, string> ForwardingShipments = new Dictionary<string, string>();

        public static void SetTableLogic(DataRow TableRow, string TableName)
        {
            
            if (TableName == "CargoTrackingShipments")
            {
                if (TableRow["ShipmentLevelCode"].Equals("D") || TableRow["ShipmentLevelCode"].Equals("H"))
                { 

                    if (!TableRow["CustomFileId"].Equals(null) && !TableRow["CustomFileId"].Equals("") &&  TableRow["CustomFileId"].GetType().Name != "DBNull")
                    {
                        ForwardingShipments.Add((string)TableRow["Id"], (string)TableRow["CustomFileId"]);
                    }
                }

            }
 

        }

 


    }

}
