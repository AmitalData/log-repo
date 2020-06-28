using CargoTracking.CargoTracking.BL.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTracking.CargoTracking.BL.Services
{
    public class CargoTrackingTableLogicService
    {

        public static void SetTableLogic(DataRow TableRow, string TableName)
        {
            if (TableName == "CargoTrackingPorts")
            {
                //if (TableRow["Code"].Equals("MUT"))
                //{
                    TableRow.SetField("EnglishName", "Cargo_Test");
                //}

            }

            if (TableName == "CargoTrackingShipments")
            {
                CompareNullabelFirstPickupETADateTime(TableRow);
                TableRow.SetField("Master", TableRow["MasterShipmentDataId"]);
                TableRow.SetField("PickupDate", TableRow["FirstPickupETA"]);
                TableRow.SetField("ClearanceDate", TableRow["CustomsClearanceDate"]);

                if (!TableRow["CustomsClearanceDate"].Equals(null))
                {
                    TableRow.SetField("ClearanceDone", true);
                }

                if (TableRow["EntityType"].Equals("C") || TableRow["ShipmentLevelCode"].Equals("F"))
                {
                    TableRow.SetField("EntityId", TableRow["Id"]);
                }
                else if (TableRow["EntityType"].Equals("O"))
                {
                    TableRow.SetField("EntityId", TableRow["Id"]);

                }
            }

            if (TableName == "CargoTrackingShipmentSearchFields")
            {
                TableRow.SetField("ShipmentDate", TableRow["CreateDateTime"]);
                TableRow.SetField("SearchFields", "");
                TableRow.SetField("ShipmentId", TableRow["Id"]);
            }

        }



        private static void CompareNullabelFirstPickupETADateTime(DataRow TableRow)
        {
            var FirstPickupETA = TableRow["FirstPickupETA"].GetType();
            if (FirstPickupETA.FullName == "System.DBNull")
            {
                TableRow.SetField("PickupDone", false);
            }
            else
            {
                DateTime? lastPostDate = (DateTime?)(TableRow["FirstPickupETA"]);
                DateTime? todayDate = DateTime.Today.Date;

                if (FirstPickupETA != null && lastPostDate.Value.Date < todayDate.Value.Date)
                {
                    TableRow.SetField("PickupDone", true);
                }
                else
                {
                    TableRow.SetField("PickupDone", false);

                }
            }

        }


    }

}
