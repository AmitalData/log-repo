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

        public static void SetTableLogic(SetTableLogicArgs args)
        {

            SetCustomerReference(args.TableRow);
            SetMilestonesFields(args);
            if (args.ConditionNumber == ShipmentTable_GetAllCustomsShipmentsThatContainForwardingShipments)
                SetForwardingShipmentHeaderId(args.TableRow);
            else if (args.ConditionNumber == ShipmentTable_GetAllNonCustomShipmentsThatContainForwardingShipments)
                SetFieldsForCustomShipment(args.TableRow);
            SetFieldsForForwardingShipment(args.TableRow);
            SetFieldsForCustomShipment(args.TableRow);
            SetGrossWeightUnit(args.TableRow);
            SetCurrentMilestone(args);
            SetShipmentTypeCode(args.TableRow);
            SetExceptionDescription(args.TableRow);
            SetHouse(args.TableRow);

        }

        private static void SetHouse(DataRow tableRow)
        {
            tableRow.SetField("SHOHouse", tableRow["OrderHouse"]);
        }

        private static void SetDefaultFields(DataRow tableRow)
        {
            SetDefaultShipper(tableRow);
            SetDefaultConsignee(tableRow);
        }


        
        private static void SetMilestonesFields(SetTableLogicArgs args)
        {
            SetCreatedMilstones(args);
            SetPickupMilestones(args);
            SetFromWarehouseMilestones(args);
            SetToWarehouseMilestones(args);
            SetDepartureMilestones(args);
            SetArrivalMilestones(args);
            SetCustomsPaymentMilestones(args);
            SetClearanceMilestones(args);
            SetDeliveredMilestones(args);
            SetDeliveryMilestones(args);
            SetTruckerMilestoneFields(args);
            SetCustomAgentFields(args);
            SetGoodsClassificationMilestone(args);
            SetDocumentInspectionMilestone(args);
            SetPaymentRequestedMilestone(args);
            SetPaymentReceivedMilestone(args);
            SetGatepassDocumentsReadyMilestone(args);
            SetInvoicedMilestone(args);
            SetBookingMilestone(args);

        }
        private static void SetCreatedMilstones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Created, tenant))
            {
                tableRow.SetField("CreateDate", (DBNull)null);
                tableRow.SetField("CreatedDone", false);
                return;
            }
            tableRow.SetField("CreateDate", tableRow["CreateDateTime"]);
            tableRow.SetField("CreatedDone", !IsFieldNullOrEmpty(tableRow, "CreateDate"));
        }
        private static void SetCustomAgentFields(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.AssignedToCustomsBroker, tenant))
            {
                tableRow.SetField("AssignedCustomsAgentDate", (DBNull)null);
                tableRow.SetField("AssignedCustomsAgentDone", false);
                return;
            }
            tableRow.SetField("AssignedCustomsAgentDate", tableRow["AssginedToCustomsAgentDate"]);
            tableRow.SetField("AssignedCustomsAgentDone", !IsFieldNullOrEmpty(tableRow, "AssignedCustomsAgentDate"));
        }

        private static void SetDeliveredMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Delivered, tenant))
            {
                tableRow.SetField("DeliveredEstimationDate", (DBNull)null);
                tableRow.SetField("DeliveredDate", (DBNull)null);
                tableRow.SetField("DeliveredDone", false);
                return;
            }

            tableRow.SetField("DeliveredEstimationDate", tableRow["FinalDeliveryETA"]);
            tableRow.SetField("DeliveredDate", tableRow["FinalDeliveryATA"]);
            SetDeliveredDone(tableRow);


        }
        private static void SetTruckerMilestoneFields(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.AssignedToTrucker, tenant))
            {
                tableRow.SetField("AssignedTruckerDate", (DBNull)null);
                tableRow.SetField("AssignedTruckerDone", false);
                return;
            }
            tableRow.SetField("AssignedTruckerDate", tableRow["AssignedToTruckerDate"]);
            tableRow.SetField("AssignedTruckerDone", !IsFieldNullOrEmpty(tableRow, "AssignedToTruckerDate"));
        }
        private static void SetDeliveryMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.DeliveryOut, tenant))
            {
                tableRow.SetField("DeliveryEstimationDate", (DBNull)null);
                tableRow.SetField("DeliveryDate", (DBNull)null);
                tableRow.SetField("DeliveryDone", false);
                tableRow.SetField("DeliveryNotes", (DBNull)null);
                return;
            }
            tableRow.SetField("DeliveryEstimationDate", tableRow["FinalDeliveryETD"]);
            tableRow.SetField("DeliveryDate", tableRow["FinalDeliveryATD"]);
            SetDeliveryDone(tableRow);
            SetDeliveryNotes(tableRow);
        }
        private static void SetClearanceMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Clearance, tenant))
            {
                tableRow.SetField("ClearanceDate", (DBNull)null);
                tableRow.SetField("ClearanceDone", false);
                return;
            }
            tableRow.SetField("ClearanceDate", tableRow["CustomsClearanceDate"]);
            SetClearanceDone(tableRow);
        }
        private static void SetCustomsPaymentMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.CustomsPayment, tenant))
            {
                tableRow.SetField("CustomsPaymentDate", (DBNull)null);
                tableRow.SetField("CustomsPaymentDone", false);
                return;
            }
            tableRow.SetField("CustomsPaymentDate", tableRow["DeclarationDate"]);
            SetCustomsPaymentDone(tableRow);

        }
        private static void SetArrivalMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Arrival, tenant))
            {
                tableRow.SetField("ArrivalEstimationDate", (DBNull)null);
                tableRow.SetField("ArrivalDate", (DBNull)null);
                tableRow.SetField("ArrivalDone", false);
                return;
            }
            tableRow.SetField("ArrivalEstimationDate", tableRow["MainCarriageETA"]);
            tableRow.SetField("ArrivalDate", tableRow["MainCarriageATA"]);
            SetArrivalDone(tableRow);

        }
        private static void SetDepartureMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Departure, tenant))
            {
                tableRow.SetField("DepartureEstimationDate", (DBNull)null);
                tableRow.SetField("DepartureDate", (DBNull)null);
                tableRow.SetField("DepartureDone", false);
                return;
            }
            tableRow.SetField("DepartureEstimationDate", tableRow["MainCarriageETD"]);
            tableRow.SetField("DepartureDate", tableRow["MainCarriageATD"]);
            SetDepartureDone(tableRow);
        }
        private static void SetToWarehouseMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.ToWarehouse, tenant))
            {
                tableRow.SetField("ToWarehouseDate", (DBNull)null);
                tableRow.SetField("ToWarehouseNotes", (DBNull)null);
                tableRow.SetField("ToWarehouseEstimationDate", (DBNull)null);
                tableRow.SetField("ToWarehouseDone", false);
                return;
            }
            SetToWarehouseDate(tableRow);
            SetToWarehouseNotes(tableRow);
            SetToWarehouseEstimationDate(tableRow);
            SetToWarehouseDone(tableRow);



        }
        private static void SetFromWarehouseMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.FromWarehouse, tenant))
            {
                tableRow.SetField("FromWarehouseDate", (DBNull)null);
                tableRow.SetField("FromWarehouseEstimationDate", (DBNull)null);
                tableRow.SetField("FromWarehouseNotes", (DBNull)null);
                tableRow.SetField("FromWarehouseDone", false);
                return;
            }

            SetFromWarehouseDate(tableRow);
            SetFromWarehouseEstimationDate(tableRow);
            SetFromWarehouseNotes(tableRow);
            SetFromWarehouseDoneField(tableRow);


        }
        private static void SetPickupMilestones(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Pickup, tenant))
            {
                tableRow.SetField("PickupEstimationDate", (DBNull)null);
                tableRow.SetField("PickupDate", (DBNull)null);
                SetPickupDoneField(tableRow);
                return;
            }
            tableRow.SetField("PickupEstimationDate", tableRow["FirstPickupETD"]);
            tableRow.SetField("PickupDate", tableRow["FirstPickupATD"]);
            SetPickupDoneField(tableRow);

        }

        private static void SetCustomerReference(DataRow tableRow)
        {
            var customerReferences = new List<string>();
            AddCustomerReferences(customerReferences, tableRow, "CustomerReference3");
            AddCustomerReferences(customerReferences, tableRow, "ForwardingCustomerReference3");
            AddCustomerReferences(customerReferences, tableRow, "OrderCustomerReference");
            AddCustomerReferences(customerReferences, tableRow, "OrderPoNumber");
            AddCustomerReferences(customerReferences, tableRow, "OrderBookingNumber");
            tableRow.SetField("CustomerReference", string.Join(",", customerReferences));
        }

        private static void AddCustomerReferences(List<string> customerReferences, DataRow tableRow, string referenceName)
        {
            if (!tableRow.IsNull(referenceName) && !tableRow[referenceName].Equals(""))
            {
                customerReferences.AddRange(GetCustomerReferences(tableRow[referenceName].ToString()));
            }
        }

        private static List<string> GetCustomerReferences(string references)
        {
            var customerReferences = new List<string>();
            var splitedCustomerReferences = references.Split(',');
            if(splitedCustomerReferences.Length == 0)
            {
                return customerReferences;
            }
            foreach (var item in splitedCustomerReferences)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    customerReferences.Add(item);
                }
            }
            return customerReferences;
        }

        private static void SetGrossWeightUnit(DataRow tableRow)
        {

            tableRow.SetField("GrossWeightUnitCode", tableRow["GrossWeightUnitCode"]);
        }
        private static void SetCurrentMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var currentMilestoneArgs = new CheckCurrentMilestoneArgs(tableRow);
            foreach (var milestone in args.Milestones)
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
                            currentMilestoneArgs.date = tableRow["FromWarehouseDate"];
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
                    case CargoTrackingMilestoneValues.Invoiced:
                        if (!IsFieldNullOrEmpty(tableRow, "InvoicedDone") && !tableRow["InvoicedDone"].Equals("False"))
                        {
                            currentMilestoneArgs.date = tableRow["InvoicedDate"];
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
        private static void SetGoodsClassificationMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.GoodsClassification, tenant))
            {
                tableRow.SetField("GoodsClassificationDate", (DBNull)null);
                tableRow.SetField("GoodsClassificationDone", false);
                return;
            }
            tableRow.SetField("GoodsClassificationDate", tableRow["GoodsClassification"]);
            tableRow.SetField("GoodsClassificationDone", !IsFieldNullOrEmpty(tableRow, "GoodsClassification"));

        }
        private static void SetDocumentInspectionMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.DocumentInspection, tenant))
            {
                tableRow.SetField("DocumentInspectionDate", (DBNull)null);
                tableRow.SetField("DocumentInspectionDone", false);
                return;
            }
            tableRow.SetField("DocumentInspectionDate", tableRow["DocumentInspection"]);
            tableRow.SetField("DocumentInspectionDone", !IsFieldNullOrEmpty(tableRow, "DocumentInspection"));

        }
        private static void SetPaymentRequestedMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.PaymentRequested, tenant))
            {
                tableRow.SetField("PaymentRequiredDate", (DBNull)null);
                tableRow.SetField("PaymentRequiredDone", false);
                return;
            }

            tableRow.SetField("PaymentRequiredDate", tableRow["PaymentRequestDateTime"]);
            tableRow.SetField("PaymentRequiredDone",!IsFieldNullOrEmpty(tableRow, "PaymentRequestDateTime") );
        }

        private static void SetPaymentReceivedMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.PaymentReceived, tenant))
            {
                tableRow.SetField("PaymentReceivedDate", (DBNull)null);
                tableRow.SetField("PaymentReceivedDone", false);
                return;
            }
            tableRow.SetField("PaymentReceivedDate", tableRow["PaymentDateTime"]);
            tableRow.SetField("PaymentReceivedDone", !IsFieldNullOrEmpty(tableRow, "PaymentDateTime"));

        }
        private static void SetGatepassDocumentsReadyMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.GatepassArrived, tenant))
            {
                tableRow.SetField("GatepassArrivedDate", (DBNull)null);
                tableRow.SetField("GatepassArrivedDone", false);
                return;
            }
            tableRow.SetField("GatepassArrivedDate", tableRow["GatepassDocumentsReady"]);
            tableRow.SetField("GatepassArrivedDone", !IsFieldNullOrEmpty(tableRow, "GatepassDocumentsReady"));

        }
        private static void SetInvoicedMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var tenant = (int)tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Invoiced, tenant))
            {
                tableRow.SetField("InvoicedDate", (DBNull)null);
                tableRow.SetField("InvoicedDone", false);
                return;
            }
            tableRow.SetField("InvoicedDate", tableRow["InvoiceIssuedDate"]);
            tableRow.SetField("InvoicedDone", !IsFieldNullOrEmpty(tableRow, "InvoiceIssuedDate"));

        }

        private static void SetBookingMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            tableRow.SetField("BookingDone", !IsFieldNullOrEmpty(tableRow, "BookingDate"));
        }
        
    }
}
