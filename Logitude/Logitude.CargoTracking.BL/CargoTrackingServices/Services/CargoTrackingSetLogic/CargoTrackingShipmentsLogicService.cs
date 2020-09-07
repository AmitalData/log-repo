using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingCardsLogicService
    {
        public static void SetTableLogic(DataRow TableRow, int ConditionNumber)
        {
            CompareNullabelFirstPickupETADateTime(TableRow);
            TableRow.SetField("Master", TableRow["MasterShipmentDataId"]);
            TableRow.SetField("PickupEstimationDate", TableRow["FirstPickupETD"]);
            TableRow.SetField("ClearanceDate", TableRow["CustomsClearanceDate"]);
            TableRow.SetField("CreateDate", TableRow["CreateDateTime"]);
            TableRow.SetField("DepartureEstimationDate", TableRow["MainCarriageETD"]);
            TableRow.SetField("ArrivalEstimationDate", TableRow["MainCarriageETA"]);
            TableRow.SetField("DeliveredEstimationDate", TableRow["FinalDeliveryETA"]);
            TableRow.SetField("PickupDate", TableRow["FirstPickupATD"]);
            SetFromWarehouseDoneField(TableRow);
            SetPickupDoneField(TableRow);
            SetFromWarehouseDate(TableRow);
            SetFromWarehouseEstimationDate(TableRow);
            SetFromWarehouseNotes(TableRow);
            TableRow.SetField("DepartureDate", TableRow["MainCarriageATD"]);
            SetDepartureDone(TableRow);
            TableRow.SetField("ArrivalDate", TableRow["MainCarriageATA"]);
            SetArrivalDone(TableRow);
            SetToWarehouseDate(TableRow);
            SetToWarehouseDone(TableRow);
            SetToWarehouseEstimationDate(TableRow);
            SetToWarehouseNotes(TableRow);
            TableRow.SetField("DeliveredDate", TableRow["FinalDeliveryATA"]);
            SetDeliveredDone(TableRow);
            TableRow.SetField("CustomsPaymentDate", TableRow["DeclarationDate"]);
            SetCustomsPaymentDone(TableRow);
            TableRow.SetField("ClearanceDate", TableRow["CustomsClearanceDate"]);
            SetClearanceDone(TableRow);
            SetCustomerReference(TableRow);
            SetClearance(TableRow);
            if (ConditionNumber == 1)
            {
                SetFieldsForCustomShipment(TableRow);
            }
            else if (ConditionNumber == 2)
            {
                SetFieldsForForwardingShipment(TableRow);
            }
            SetCurrentMilestone(TableRow);

        }

        private static void SetCustomerReference(DataRow TableRow)
        {
            if (!TableRow["CustomerReference1"].Equals(null) && !TableRow["CustomerReference1"].Equals("") && TableRow["CustomerReference1"].GetType().Name != "DBNull" && !TableRow["CustomerReference2"].Equals(null) && !TableRow["CustomerReference2"].Equals("") && TableRow["CustomerReference2"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomerReference", TableRow["CustomerReference1"] + "," + TableRow["CustomerReference2"]);
            }
            else if (!TableRow["CustomerReference1"].Equals(null) && TableRow["CustomerReference1"].GetType().Name != "" && TableRow["CustomerReference1"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomerReference", TableRow["CustomerReference1"]);
            }
            else if (!TableRow["CustomerReference2"].Equals(null) && TableRow["CustomerReference2"].GetType().Name != "" && TableRow["CustomerReference2"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomerReference", TableRow["CustomerReference2"]);
            }
        }

        private static void SetClearance(DataRow TableRow)
        {
            if (!TableRow["CustomsClearanceDate"].Equals(null) && TableRow["CustomsClearanceDate"].GetType().Name != "DBNull")
            {
                TableRow.SetField("ClearanceDone", true);
            }
            else
            {
                TableRow.SetField("ClearanceDone", false);

            }
        }
        private static void SetCurrentMilestone(DataRow TableRow)
        {
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
        }
        private static void SetFieldsForCustomShipment(DataRow TableRow)
        {
            TableRow.SetField("EntityType", "C");
            TableRow.SetField("EntityId", TableRow["Id"]);
            TableRow.SetField("IsMainRecord", true);
            TableRow.SetField("ForwardingShipmentHeaderId", TableRow["ForwardingIdForCustom"]);


        }

        private static void SetFieldsForForwardingShipment(DataRow TableRow)
        {
            TableRow.SetField("EntityType", "F");
            TableRow.SetField("EntityId", TableRow["Id"]);

            if (!TableRow["CustomFileId"].Equals(null) && !TableRow["CustomFileId"].Equals("") && TableRow["CustomFileId"].GetType().Name != "DBNull")
            {
                TableRow.SetField("CustomsShipmentHeaderId", TableRow["CustomFileId"]);
                TableRow.SetField("IsMainRecord", false);
                //ForwardingShipments.Add((string)TableRow["Id"], (string)TableRow["CustomFileId"]);
            }
            else
            {
                TableRow.SetField("IsMainRecord", true);

            }
        }

        private static void SetFromWarehouseDoneField(DataRow TableRow)
        {
            TableRow.SetField("FromWarehouseDone", false);

            if (TableRow["DirectionId"].Equals("E"))
            {
                if (!IsFieldNullOrEmpty(TableRow, "WarehouseLegActualEntryDate"))
                {
                    DateTime? WarehouseLegActualEntryDate = (DateTime?)(TableRow["WarehouseLegActualEntryDate"]);
                    DateTime? todayDate = DateTime.Today.Date;

                    if (WarehouseLegActualEntryDate != null && WarehouseLegActualEntryDate.Value.Date <= todayDate.Value.Date)
                    {
                        TableRow.SetField("FromWarehouseDone", true);
                    }
                }
            }

        }

        private static void SetPickupDoneField(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "PickupDate"))
            {
                TableRow.SetField("PickupDone", true);
            }
            else
            {
                TableRow.SetField("PickupDone", false);

            }


        }


        private static void SetDepartureDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "DepartureDate"))
            {
                TableRow.SetField("DepartureDone", true);
            }
            else
            {
                TableRow.SetField("DepartureDone", false);

            }


        }

        private static void SetToWarehouseDone(DataRow TableRow)
        {
            TableRow.SetField("ToWarehouseDone", false);
            if (TableRow["DirectionId"].Equals("I"))
            {
                if (!IsFieldNullOrEmpty(TableRow, "ToWarehouseDate"))
                {
                    TableRow.SetField("ToWarehouseDone", true);
                }

            }


        }

        private static void SetClearanceDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "ClearanceDate"))
            {
                TableRow.SetField("ClearanceDone", true);
            }
            else
            {
                TableRow.SetField("ClearanceDone", false);

            }


        }

        private static void SetCustomsPaymentDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "CustomsPaymentDate"))
            {
                TableRow.SetField("CustomsPaymentDone", true);
            }
            else
            {
                TableRow.SetField("CustomsPaymentDone", false);

            }


        }

        private static void SetDeliveredDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "DeliveredDate"))
            {
                TableRow.SetField("DeliveredDone", true);
            }
            else
            {
                TableRow.SetField("DeliveredDone", false);

            }


        }

        private static void SetArrivalDone(DataRow TableRow)
        {
            if (!IsFieldNullOrEmpty(TableRow, "ArrivalDate"))
            {
                TableRow.SetField("ArrivalDone", true);
            }
            else
            {
                TableRow.SetField("ArrivalDone", false);

            }


        }


        private static void SetFromWarehouseDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("E"))
            {
                TableRow.SetField("FromWarehouseDate", TableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetFromWarehouseEstimationDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("E"))
            {
                TableRow.SetField("FromWarehouseEstimationDate", TableRow["WarehouseLegExpectedEntryDate"]);
            }


        }

        private static void SetToWarehouseDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("I"))
            {
                TableRow.SetField("ToWarehouseDate", TableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetToWarehouseEstimationDate(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("I"))
            {
                TableRow.SetField("ToWarehouseEstimationDate", TableRow["WarehouseLegExpectedEntryDate"]);
            }


        }



        private static void SetToWarehouseNotes(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("I"))
            {
                TableRow.SetField("ToWarehouseNotes", TableRow["WarehouseLegRemarks"]);
            }


        }

        private static void SetFromWarehouseNotes(DataRow TableRow)
        {
            if (TableRow["DirectionId"].Equals("E"))
            {
                TableRow.SetField("FromWarehouseNotes", TableRow["WarehouseLegRemarks"]);
            }


        }

        private static bool IsFieldNullOrEmpty(DataRow TableRow, string CoulmnName)
        {
            bool IsNull = false;
            if (TableRow[CoulmnName].Equals(null) || TableRow[CoulmnName].Equals("") || TableRow[CoulmnName].GetType().Name == "DBNull")
            {
                IsNull = true;
            }

            return IsNull;


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
