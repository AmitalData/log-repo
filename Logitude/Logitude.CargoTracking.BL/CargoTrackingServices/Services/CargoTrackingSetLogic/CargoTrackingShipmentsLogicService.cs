using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentsLogicService : ShareTableLogic
    {
        const int ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments = 1;
        const int ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments = 2;
        const string defaultShipperId = "DF-SHIPPER";
        const string defaultConsigneeId = "DF-CONSIGNE";
        const string exportDirection = "E";
        const string importDirection = "I";
        const string customsDirection = "C";

        public static void SetTableLogic(DataRow tableRow, int conditionNumber, List<CargoTrackingMilestoneList> milestoneList)
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
            SetCurrentMilestone(tableRow, milestoneList);
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
            SetCreatedMilstones(tableRow);
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

            SetGoodsClassificationMilestone(tableRow);
            SetDocumentInspectionMilestone(tableRow);

            SetPaymentRequestedMilestone(tableRow);
            SetPaymentReceivedMilestone(tableRow);

            SetGatepassDocumentsReadyMilestone(tableRow);
        }
        private static void SetCreatedMilstones(DataRow tableRow)
        {
            tableRow.SetField("CreateDate", tableRow["CreateDate"]);
            tableRow.SetField("CreatedDone", !IsFieldNullOrEmpty(tableRow, "CreateDate"));
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
            SetDeliveryNotes(tableRow);

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
            if (!tableRow.IsNull("CustomerReference1") && !tableRow["CustomerReference1"].Equals("") && !tableRow.IsNull("CustomerReference2") && !tableRow["CustomerReference2"].Equals(""))
            {
                tableRow.SetField("CustomerReference", tableRow["CustomerReference1"] + "," + tableRow["CustomerReference2"]);
            }
            else if (!tableRow.IsNull("CustomerReference1") && tableRow["CustomerReference1"].GetType().Name != "")
            {
                tableRow.SetField("CustomerReference", tableRow["CustomerReference1"]);
            }
            else if (!tableRow.IsNull("CustomerReference2") && tableRow["CustomerReference2"].GetType().Name != "")
            {
                tableRow.SetField("CustomerReference", tableRow["CustomerReference2"]);
            }
        }

        private static void SetGrossWeightUnit(DataRow tableRow)
        {

            tableRow.SetField("GrossWeightUnitCode", tableRow["GrossWeightUnitCode"]);
        }
        private static void SetCurrentMilestone(DataRow tableRow, List<Data.EntityLists.CargoTrackingMilestoneList> milestoneList)
        {
            var currentMilestoneArgs = new CheckCurrentMilestoneArgs(tableRow);
            foreach (var milestone in milestoneList)
            {
                currentMilestoneArgs.milestone = milestone;
                switch (milestone.Code)
                {
                    case CargoTrackingMilestoneValues.Created:
                        if (!IsFieldNullOrEmpty(tableRow, "CreatedDone") && !tableRow["CreatedDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["CreateDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    //case CargoTrackingMilestoneValues.Booking:
                    //    if (!IsFieldNullOrEmpty(tableRow, "BookingDone") && !tableRow["BookingDone"].Equals("False"))
                    //    {
                    //        currentMilestoneArgs.date = tableRow["BookingDate"];
                    //        CheckMilestone(currentMilestoneArgs);
                    //    }
                    //    break;
                    case CargoTrackingMilestoneValues.Pickup:
                        if (!IsFieldNullOrEmpty(tableRow, "PickupDone") && !tableRow["PickupDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["PickupDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.FromWarehouse:
                        if (!IsFieldNullOrEmpty(tableRow, "FromWarehouseDone") && !tableRow["FromWarehouseDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow[""];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.Departure:
                        if (!IsFieldNullOrEmpty(tableRow, "DepartureDone") && !tableRow["DepartureDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["DepartureDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.Arrival:
                        if (!IsFieldNullOrEmpty(tableRow, "ArrivalDone") && !tableRow["ArrivalDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["ArrivalDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.ToWarehouse:
                        if (!IsFieldNullOrEmpty(tableRow, "ToWarehouseDone") && !tableRow["ToWarehouseDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["ToWarehouseDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.AssignedToCustomsBroker:
                        if (!IsFieldNullOrEmpty(tableRow, "AssignedCustomsAgentDone") && !tableRow["AssignedCustomsAgentDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["AssignedCustomsAgentDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.GoodsClassification:
                        if (!IsFieldNullOrEmpty(tableRow, "GoodsClassificationDone") && !tableRow["GoodsClassificationDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["GoodsClassificationDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.DocumentInspection:
                        if (!IsFieldNullOrEmpty(tableRow, "DocumentInspectionDone") && !tableRow["DocumentInspectionDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["DocumentInspectionDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.PaymentRequested:
                        if (!IsFieldNullOrEmpty(tableRow, "PaymentRequiredDone") && !tableRow["PaymentRequiredDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["PaymentRequiredDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.PaymentReceived:
                        if (!IsFieldNullOrEmpty(tableRow, "PaymentReceivedDone") && !tableRow["PaymentReceivedDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["PaymentReceivedDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.CustomsPayment:
                        if (!IsFieldNullOrEmpty(tableRow, "CustomsPaymentDone") && !tableRow["CustomsPaymentDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["CustomsPaymentDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.Clearance:
                        if (!IsFieldNullOrEmpty(tableRow, "ClearanceDone") && !tableRow["ClearanceDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["ClearanceDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.GatepassArrived:
                        if (!IsFieldNullOrEmpty(tableRow, "GatepassArrivedDone") && !tableRow["GatepassArrivedDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["GatepassArrivedDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.AssignedToTrucker:
                        if (!IsFieldNullOrEmpty(tableRow, "AssignedTruckerDone") && !tableRow["AssignedTruckerDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["AssignedTruckerDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.DeliveryOut:
                        if (!IsFieldNullOrEmpty(tableRow, "DeliveryDone") && !tableRow["DeliveryDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["DeliveryDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;
                    case CargoTrackingMilestoneValues.Delivered:
                        if (!IsFieldNullOrEmpty(tableRow, "DeliveredDone") && !tableRow["DeliveredDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["DeliveredDate"];
                            CheckMilestone(currentMilestoneArgs);
                        }
                        break;

                    default:
                        break;
                }
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
                tableRow.SetField("EntityType", customsDirection);
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

            if (tableRow["DirectionId"].Equals(exportDirection))
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
            if (tableRow["DirectionId"].Equals(importDirection) || tableRow["DirectionId"].Equals(customsDirection))
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
            if (tableRow["DirectionId"].Equals(exportDirection))
            {
                tableRow.SetField("FromWarehouseDate", tableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetFromWarehouseEstimationDate(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals(exportDirection))
            {
                tableRow.SetField("FromWarehouseEstimationDate", tableRow["WarehouseLegExpectedEntryDate"]);
            }


        }

        private static void SetToWarehouseDate(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals(importDirection) || tableRow["DirectionId"].Equals(customsDirection))
            {
                tableRow.SetField("ToWarehouseDate", tableRow["WarehouseLegActualEntryDate"]);
            }


        }


        private static void SetToWarehouseEstimationDate(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals(importDirection) || tableRow["DirectionId"].Equals(customsDirection))
            {
                tableRow.SetField("ToWarehouseEstimationDate", tableRow["WarehouseLegExpectedEntryDate"]);
            }


        }



        private static void SetToWarehouseNotes(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals(importDirection))
            {
                tableRow.SetField("ToWarehouseNotes", tableRow["WarehouseLegRemarks"]);
            }


        }

        private static void SetFromWarehouseNotes(DataRow tableRow)
        {
            if (tableRow["DirectionId"].Equals(exportDirection))
            {
                tableRow.SetField("FromWarehouseNotes", tableRow["WarehouseLegRemarks"]);
            }


        }

        private static void SetDeliveryNotes(DataRow tableRow)
        {
            var isLocalNameExist = !IsFieldNullOrEmpty(tableRow, "CarrierLocalName");
            var isEnglishNameExist = !IsFieldNullOrEmpty(tableRow, "CarrierEnglishName");

            if (isLocalNameExist || isEnglishNameExist)
            {
                var carrierName = isLocalNameExist ? tableRow["CarrierLocalName"] : tableRow["CarrierEnglishName"];
                tableRow.SetField("DeliveryNotes", "Via " + carrierName);
            }
        }

        private static bool IsFieldNullOrEmpty(DataRow tableRow, string coulmnName)
        {
            if (tableRow[coulmnName].Equals(null) || tableRow[coulmnName].Equals("") || tableRow[coulmnName].GetType().Name == "DBNull")
                return true;
            return false;
        }

        private static void SetShipmentTypeCode(DataRow tableRow)
        {
            tableRow.SetField("ShipmentTypeCode", tableRow["ShipmentTypeId"]);
        }
        private static void SetExceptionDescription(DataRow tableRow)
        {
            if (tableRow["ClearanceDone"].Equals("False"))
            {
                var exceptionDate = tableRow["ExceptionDate"]?.ToString();
                tableRow.SetField("CurrentMilestoneExceptions", string.IsNullOrWhiteSpace(exceptionDate) ? tableRow["ExceptionDescription"] : tableRow["ExceptionDate"] + "," + tableRow["ExceptionDescription"]);
            }
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
        private static void SetGoodsClassificationMilestone(DataRow tableRow)
        {
            tableRow.SetField("GoodsClassificationDate", tableRow["GoodsClassification"]);
            tableRow.SetField("GoodsClassificationDone", !IsFieldNullOrEmpty(tableRow, "GoodsClassification"));
        }
        private static void SetDocumentInspectionMilestone(DataRow tableRow)
        {
            tableRow.SetField("DocumentInspectionDate", tableRow["DocumentInspection"]);
            tableRow.SetField("DocumentInspectionDone", !IsFieldNullOrEmpty(tableRow, "DocumentInspection"));
        }
        private static void SetPaymentRequestedMilestone(DataRow tableRow)
        {
            var isPaymentRequiredDateFilled = IsFieldNullOrEmpty(tableRow, "PaymentDateTime") && tableRow["IsPaymentRequired"].Equals(true);
            tableRow.SetField("PaymentRequiredDate", isPaymentRequiredDateFilled ? tableRow["PaymentRequestDateTime"]: null);
            tableRow.SetField("PaymentRequiredDone", isPaymentRequiredDateFilled ? !IsFieldNullOrEmpty(tableRow, "PaymentRequestDateTime") : false);
        }

        private static void SetPaymentReceivedMilestone(DataRow tableRow)
        {
            tableRow.SetField("PaymentReceivedDate", tableRow["PaymentDateTime"]);
            tableRow.SetField("PaymentReceivedDone", !IsFieldNullOrEmpty(tableRow, "PaymentDateTime"));
        }
        private static void SetGatepassDocumentsReadyMilestone(DataRow tableRow)
        {
            tableRow.SetField("GatepassArrivedDate", tableRow["GatepassDocumentsReady"]);
            tableRow.SetField("GatepassArrivedDone", !IsFieldNullOrEmpty(tableRow, "GatepassDocumentsReady"));
        }

    }
}
