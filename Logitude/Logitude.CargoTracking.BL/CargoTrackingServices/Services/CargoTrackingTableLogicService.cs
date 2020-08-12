 
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
                if (TableRow["Code"].Equals("MUT"))
                {
                    TableRow.SetField("EnglishName", "Cargo_Test");
                }

            }

            if (TableName == "CargoTrackingShipments")
            {
                CompareNullabelFirstPickupETADateTime(TableRow);
                TableRow.SetField("Master", TableRow["MasterShipmentDataId"]);
                TableRow.SetField("PickupDate", TableRow["FirstPickupETA"]);
                TableRow.SetField("ClearanceDate", TableRow["CustomsClearanceDate"]);
                TableRow.SetField("CreateDate", TableRow["CreateDateTime"]);
 
                if (!TableRow["CustomerReference1"].Equals(null) && !TableRow["CustomerReference1"].Equals("") && TableRow["CustomerReference1"].GetType().Name != "DBNull" && !TableRow["CustomerReference2"].Equals(null) && !TableRow["CustomerReference2"].Equals("") && TableRow["CustomerReference2"].GetType().Name != "DBNull")
                {
                    TableRow.SetField("CustomerReference", TableRow["CustomerReference1"] + "," + TableRow["CustomerReference2"]);
                }
                else if(!TableRow["CustomerReference1"].Equals(null) && TableRow["CustomerReference1"].GetType().Name != "" && TableRow["CustomerReference1"].GetType().Name != "DBNull")
                {
                    TableRow.SetField("CustomerReference", TableRow["CustomerReference1"] );

                }
                else if (!TableRow["CustomerReference2"].Equals(null) && TableRow["CustomerReference2"].GetType().Name != "" && TableRow["CustomerReference2"].GetType().Name != "DBNull")
                {
                    TableRow.SetField("CustomerReference", TableRow["CustomerReference2"]);

                }
                //TableRow.SetField("ShipmentId", TableRow["Id"]);

                if (!TableRow["CustomsClearanceDate"].Equals(null) && TableRow["CustomsClearanceDate"].GetType().Name != "DBNull")
                {
                    TableRow.SetField("ClearanceDone", true);
                }
                else
                {
                    TableRow.SetField("ClearanceDone", false);

                }

                if (TableRow["ShipmentLevelCode"].Equals("D") || TableRow["ShipmentLevelCode"].Equals("H"))
                {
                    TableRow.SetField("EntityType", "F");
                    TableRow.SetField("EntityId", TableRow["Id"]);

                    if (!TableRow["CustomFileId"].Equals(null) && !TableRow["CustomFileId"].Equals("") &&  TableRow["CustomFileId"].GetType().Name != "DBNull")
                    {
                        TableRow.SetField("CustomsShipmentHeaderId", TableRow["Id"]);
                        //ForwardingShipments.Add((string)TableRow["Id"], (string)TableRow["CustomFileId"]);
                    }
                }
                else if (TableRow["ShipmentLevelCode"].Equals("A"))
                {
                    TableRow.SetField("EntityType", "C");
                    TableRow.SetField("EntityId", TableRow["Id"]);

                }

                if (!TableRow["ClearanceDone"].Equals(null) && TableRow["ClearanceDone"].GetType().Name != "DBNull" && !TableRow["ClearanceDone"].Equals("False"))
                {
                    TableRow.SetField("CurrentMilestoneCode", "9");
                    TableRow.SetField("CurrentMilestoneDate", TableRow["CustomsClearanceDate"]);

                }
                else if (!TableRow["PickupDone"].Equals(null) && TableRow["PickupDone"].GetType().Name != "DBNull" && !TableRow["PickupDone"].Equals("False"))
                {
                    TableRow.SetField("CurrentMilestoneCode", "2");
                    TableRow.SetField("CurrentMilestoneDate", TableRow["PickupDate"]);

                }
                if (ConditionNumber==1)
                {
                    TableRow.SetField("ForwardingShipmentHeaderId", TableRow["ForwardingIdForCustom"]);
                }

                //if (ForwardingShipments.Count() > 0)
                //{
                //    string TargetKey = null;
                //    foreach (KeyValuePair<string, string> entry in ForwardingShipments)
                //    {
                //        if (entry.Value.Equals(TableRow["Id"]))
                //        {
                //            TableRow.SetField("ForwardingShipmentHeaderId", entry.Key);
                //            TargetKey = entry.Key;
                //            break;
                //        }
                //    }
                //    if (!string.IsNullOrEmpty(TargetKey))
                //    {
                //        ForwardingShipments.Remove(TargetKey);
                //    }
                //}
            }

            if (TableName == "CargoTrackingShipmentSearches")
            {
                TableRow.SetField("ShipmentDate", TableRow["CreateDateTime"]);
                string Id = (string)TableRow["Id"];
                string[] SplittedId = Id.Split('_');
                TableRow.SetField("ShipmentId", SplittedId[0]);
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
