using Logitude.CargoTracking.BL.CloseTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentsLogicService
    {
        const int ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments = 1;
        const int ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments = 2;
        const string defaultShipperId = "DF-SHIPPER";
        const string defaultConsigneeId = "DF-CONSIGNE";

        public static void SetTableLogic(DataRow tableRow, int conditionNumber)
        {

            SetCreateDate(tableRow);
            SetCustomerReference(tableRow);
            SetMilestonesFields(tableRow);
            if (conditionNumber == ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments)
                SetForwardingShipmentHeaderId(tableRow);
            else if (conditionNumber == ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments)
                SetFieldsForCustomShipment(tableRow);
            SetFieldsForForwardingShipment(tableRow);
            SetFieldsForCustomShipment(tableRow);
            SetGrossWeightUnit(tableRow);
            SetCurrentMilestone(tableRow);
            SetShipmentTypeCode(tableRow);
            SetExceptionDescription(tableRow);

        }

        private static void SetDefaultFields(DataRow tableRow)
        {
            SetDefaultShipper(tableRow);
            SetDefaultConsignee(tableRow);
        }


        private static void SetCreateDate(DataRow tableRow)
        {
            tableRow.SetField("CreateDate", tableRow["CreateDateTime"]);
        }
        private static void SetMilestonesFields(DataRow tableRow)
        {

            SetPickupMilestones(tableRow);
            SetFromWarehouseMilestones(tableRow);
            SetToWarehouseMilestones(tableRow);
            SetDepartureMilestones(tableRow);
            SetArrivalMilestones(tableRow);
            SetCustomsPaymentMilestones(tableRow);
            SetClearanceMilestones(tableRow);
            SetDeliveredMilestones(tableRow);
            SetDeliveryMilestones(tableRow);
            SetTruckerMilestoneFields(tableRow);
            SetCustomAgentFields(tableRow);

        }
        private static void SetCustomAgentFields(DataRow tableRow)
        {
            tableRow.SetField("AssignedCustomsAgentDate", tableRow["AssginedToCustomsAgentDate"]);
            tableRow.SetField("AssignedCustomsAgentDone", !IsFieldNullOrEmpty(tableRow, "AssignedCustomsAgentDate"));
        }
        private static void SetDeliveredMilestones(DataRow tableRow)
        {
            tableRow.SetField("DeliveredEstimationDate", tableRow["FinalDeliveryETA"]);
            tableRow.SetField("DeliveredDate", tableRow["FinalDeliveryATA"]);
            SetDeliveredDone(tableRow);

        }
        private static void SetTruckerMilestoneFields(DataRow tableRow)
        {
            tableRow.SetField("AssignedTruckerDate", tableRow["AssignedToTruckerDate"]);
            tableRow.SetField("AssignedTruckerDone", !IsFieldNullOrEmpty(tableRow, "AssignedToTruckerDate"));
            //tableRow.SetField("AssignedTruckerEstimationDate", tableRow["AssignedTruckerEstimationDate"]);
            //tableRow.SetField("AssignedTruckerNotes", tableRow["AssignedTruckerNotes"]);
        }
        private static void SetDeliveryMilestones(DataRow tableRow)
        {
            tableRow.SetField("DeliveryEstimationDate", tableRow["FinalDeliveryETD"]);
            tableRow.SetField("DeliveryDate", tableRow["FinalDeliveryATD"]);
            SetDeliveryDone(tableRow);

        }
        private static void SetClearanceMilestones(DataRow tableRow)
        {
            tableRow.SetField("ClearanceDate", tableRow["CustomsClearanceDate"]);
            SetClearanceDone(tableRow);
        }
        private static void SetCustomsPaymentMilestones(DataRow tableRow)
        {
            tableRow.SetField("CustomsPaymentDate", tableRow["DeclarationDate"]);
            SetCustomsPaymentDone(tableRow);
        }
        private static void SetArrivalMilestones(DataRow tableRow)
        {
            tableRow.SetField("ArrivalEstimationDate", tableRow["MainCarriageETA"]);
            tableRow.SetField("ArrivalDate", tableRow["MainCarriageATA"]);
            SetArrivalDone(tableRow);
        }
        private static void SetDepartureMilestones(DataRow tableRow)
        {
            tableRow.SetField("DepartureEstimationDate", tableRow["MainCarriageETD"]);
            tableRow.SetField("DepartureDate", tableRow["MainCarriageATD"]);
            SetDepartureDone(tableRow);
        }
        private static void SetToWarehouseMilestones(DataRow tableRow)
        {
            SetToWarehouseDate(tableRow);
            SetToWarehouseDone(tableRow);
            SetToWarehouseEstimationDate(tableRow);
            SetToWarehouseNotes(tableRow);
        }
        private static void SetFromWarehouseMilestones(DataRow tableRow)
        {
            SetFromWarehouseDoneField(tableRow);
            SetFromWarehouseDate(tableRow);
            SetFromWarehouseEstimationDate(tableRow);
            SetFromWarehouseNotes(tableRow);
        }
        private static void SetPickupMilestones(DataRow tableRow)
        {
            tableRow.SetField("PickupEstimationDate", tableRow["FirstPickupETD"]);
            tableRow.SetField("PickupDate", tableRow["FirstPickupATD"]);
            SetPickupDoneField(tableRow);
        }

        private static void SetCustomerReference(DataRow tableRow)
        {
            if (!tableRow["CustomerReference1"].Equals(null) && !tableRow["CustomerReference1"].Equals("") && tableRow["CustomerReference1"].GetType().Name != "DBNull" && !tableRow["CustomerReference2"].Equals(null) && !tableRow["CustomerReference2"].Equals("") && tableRow["CustomerReference2"].GetType().Name != "DBNull")
            {
                tableRow.SetField("CustomerReference", tableRow["CustomerReference1"] + "," + tableRow["CustomerReference2"]);
            }
            else if (!tableRow["CustomerReference1"].Equals(null) && tableRow["CustomerReference1"].GetType().Name != "" && tableRow["CustomerReference1"].GetType().Name != "DBNull")
            {
                tableRow.SetField("CustomerReference", tableRow["CustomerReference1"]);
            }
            else if (!tableRow["CustomerReference2"].Equals(null) && tableRow["CustomerReference2"].GetType().Name != "" && tableRow["CustomerReference2"].GetType().Name != "DBNull")
            {
                tableRow.SetField("CustomerReference", tableRow["CustomerReference2"]);
            }
        }

        private static void SetGrossWeightUnit(DataRow tableRow)
        {

            tableRow.SetField("GrossWeightUnitCode", tableRow["GrossWeightUnitCode"]);
        }
        private static void SetCurrentMilestone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "DeliveredDone") && !tableRow["DeliveredDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Delivered);
                tableRow.SetField("CurrentMilestoneDate", tableRow["DeliveredDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "DeliveryDone") && !tableRow["DeliveryDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.DeliveryOut);
                tableRow.SetField("CurrentMilestoneDate", tableRow["DeliveryDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "AssignedTruckerDone") && !tableRow["AssignedTruckerDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.AssignedToTrucker);
                tableRow.SetField("CurrentMilestoneDate", tableRow["AssignedTruckerDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "ClearanceDone") && !tableRow["ClearanceDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Clearance);
                tableRow.SetField("CurrentMilestoneDate", tableRow["ClearanceDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "CustomsPaymentDone") && !tableRow["CustomsPaymentDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.CustomsPayment);
                tableRow.SetField("CurrentMilestoneDate", tableRow["CustomsPaymentDate"]);

            }

            //CustomsProcess
            //AssignedToCustomsAgent
            else if (!IsFieldNullOrEmpty(tableRow, "AssignedCustomsAgentDone") && !tableRow["AssignedCustomsAgentDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.AssignedToCustomsAgent);
                tableRow.SetField("CurrentMilestoneDate", tableRow["AssignedCustomsAgentDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "ToWarehouseDone") && !tableRow["ToWarehouseDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.ToWarehouse);
                tableRow.SetField("CurrentMilestoneDate", tableRow["ToWarehouseDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "ArrivalDone") && !tableRow["ArrivalDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Arrival);
                tableRow.SetField("CurrentMilestoneDate", tableRow["ArrivalDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "DepartureDone") && !tableRow["DepartureDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Departure);
                tableRow.SetField("CurrentMilestoneDate", tableRow["DepartureDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "FromWarehouseDone") && !tableRow["FromWarehouseDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.FromWarehouse);
                tableRow.SetField("CurrentMilestoneDate", tableRow["FromWarehouseDate"]);

            }
            else if (!IsFieldNullOrEmpty(tableRow, "PickupDone") && !tableRow["PickupDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Pickup);
                tableRow.SetField("CurrentMilestoneDate", tableRow["PickupDate"]);

            }

        }
        private static void SetForwardingShipmentHeaderId(DataRow tableRow)
        {
         
            tableRow.SetField("ForwardingShipmentHeaderId", tableRow["ForwardingIdForCustom"]);
        }

        private static void SetFieldsForCustomShipment(DataRow tableRow)
        {
            if (tableRow["ShipmentLevelCode"].Equals("A"))
            {
                tableRow.SetField("EntityType", "C");
                tableRow.SetField("EntityId", tableRow["Id"]);
                tableRow.SetField("IsMainRecord", true);
            }
        }

        private static void SetFieldsForForwardingShipment(DataRow tableRow)
        {
            if (!tableRow["ShipmentLevelCode"].Equals("A"))
            {
                tableRow.SetField("EntityType", "F");
                tableRow.SetField("EntityId", tableRow["Id"]);

                if (!tableRow["CustomFileId"].Equals(null) && !tableRow["CustomFileId"].Equals("") && tableRow["CustomFileId"].GetType().Name != "DBNull")
                {
                    tableRow.SetField("CustomsShipmentHeaderId", tableRow["CustomFileId"]);
                    tableRow.SetField("IsMainRecord", false);
                }
                else
                {
                    tableRow.SetField("IsMainRecord", true);

                }
                tableRow.SetField("ForwardingShipmentNumber", DBNull.Value);
            }


        }


        private static void SetCustomsShipmentHeaderId(DataRow tableRow)
        {
            if (!tableRow["CustomFileId"].Equals(null) && !tableRow["CustomFileId"].Equals("") && tableRow["CustomFileId"].GetType().Name != "DBNull")
            {
                tableRow.SetField("CustomsShipmentHeaderId", tableRow["CustomFileId"]);
            }

        }

        private static void SetFromWarehouseDoneField(DataRow tableRow)
        {
            tableRow.SetField("FromWarehouseDone", false);

            if (tableRow["DirectionId"].Equals("E"))
            {
                if (!IsFieldNullOrEmpty(tableRow, "WarehouseLegActualEntryDate"))
                {
                    DateTime? WarehouseLegActualEntryDate = (DateTime?)(tableRow["WarehouseLegActualEntryDate"]);
                    DateTime? todayDate = DateTime.Today.Date;

                    if (WarehouseLegActualEntryDate != null && WarehouseLegActualEntryDate.Value.Date <= todayDate.Value.Date)
                    {
                        tableRow.SetField("FromWarehouseDone", true);
                    }
                }
            }

        }

        private static void SetPickupDoneField(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "PickupDate"))
            {
                tableRow.SetField("PickupDone", true);
            }
            else
            {
                tableRow.SetField("PickupDone", false);

            }


        }


        private static void SetDepartureDone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "DepartureDate"))
            {
                tableRow.SetField("DepartureDone", true);
            }
            else
            {
                tableRow.SetField("DepartureDone", false);

            }


        }

        private static void SetToWarehouseDone(DataRow tableRow)
        {
            tableRow.SetField("ToWarehouseDone", false);
            if (tableRow["DirectionId"].Equals("I"))
            {
                if (!IsFieldNullOrEmpty(tableRow, "ToWarehouseDate"))
                {
                    tableRow.SetField("ToWarehouseDone", true);
                }

            }


        }

        private static void SetClearanceDone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "ClearanceDate"))
            {
                tableRow.SetField("ClearanceDone", true);
            }
            else
            {
                tableRow.SetField("ClearanceDone", false);

            }


        }

        private static void SetCustomsPaymentDone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "CustomsPaymentDate"))
            {
                tableRow.SetField("CustomsPaymentDone", true);
            }
            else
            {
                tableRow.SetField("CustomsPaymentDone", false);

            }


        }

        private static void SetDeliveredDone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "DeliveredDate"))
            {
                tableRow.SetField("DeliveredDone", true);
            }
            else
            {
                tableRow.SetField("DeliveredDone", false);

            }


        }

        private static void SetDeliveryDone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "DeliveryDate"))
            {
                tableRow.SetField("DeliveryDone", true);
            }
            else
            {
                tableRow.SetField("DeliveryDone", false);

            }


        }

        private static void SetArrivalDone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "ArrivalDate"))
            {
                tableRow.SetField("ArrivalDone", true);
            }
            else
            {
                tableRow.SetField("ArrivalDone", false);

            }


        }


        private static void SetFromWarehouseDate(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals("E"))
            {
                tableRow.SetField("FromWarehouseDate", tableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetFromWarehouseEstimationDate(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals("E"))
            {
                tableRow.SetField("FromWarehouseEstimationDate", tableRow["WarehouseLegExpectedEntryDate"]);
            }


        }

        private static void SetToWarehouseDate(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals("I"))
            {
                tableRow.SetField("ToWarehouseDate", tableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetToWarehouseEstimationDate(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals("I"))
            {
                tableRow.SetField("ToWarehouseEstimationDate", tableRow["WarehouseLegExpectedEntryDate"]);
            }


        }



        private static void SetToWarehouseNotes(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals("I"))
            {
                tableRow.SetField("ToWarehouseNotes", tableRow["WarehouseLegRemarks"]);
            }


        }

        private static void SetFromWarehouseNotes(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals("E"))
            {
                tableRow.SetField("FromWarehouseNotes", tableRow["WarehouseLegRemarks"]);
            }


        }

        private static bool IsFieldNullOrEmpty(DataRow tableRow, string coulmnName)
        {
            bool isNull = false;
            if (tableRow[coulmnName].Equals(null) || tableRow[coulmnName].Equals("") || tableRow[coulmnName].GetType().Name == "DBNull")
            {
                isNull = true;
            }

            return isNull;


        }

        private static void SetShipmentTypeCode(DataRow tableRow)
        {
            tableRow.SetField("ShipmentTypeCode", tableRow["ShipmentTypeId"]);
        }
        private static void SetExceptionDescription(DataRow tableRow)
        {
            tableRow.SetField("CurrentMilestoneExceptions", tableRow["ExceptionDate"] + "\n" + tableRow["ExceptionDescription"]);
        }
        private static void SetDefaultShipper(DataRow tableRow)
        {
            var shipperId = tableRow["ShipperId"]?.ToString();
            if (string.IsNullOrWhiteSpace(shipperId))
                tableRow.SetField("ShipperId", defaultShipperId);
        }
        private static void SetDefaultConsignee(DataRow tableRow)
        {
            var consigneeId = tableRow["ConsigneeId"]?.ToString();
            if (string.IsNullOrWhiteSpace(consigneeId))
                tableRow.SetField("ConsigneeId", defaultConsigneeId);
        }

    }
}
