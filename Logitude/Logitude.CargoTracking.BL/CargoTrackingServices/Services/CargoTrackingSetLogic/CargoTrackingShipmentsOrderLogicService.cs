using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentsOrderLogicService : ShareTableLogic
    {
        private const string InlandTransportMode = "I";
        private const string OceanTransportMode = "O";
        private const string AirTransportMode = "A";
        private const string InlandShipmentType = "FTL";
        private const string OceanShipmentType = "FCLD";
        private const string AirShipmentType = "AIR";
        private const string ShipmentOrderEntityType = "O";
        private const string CreateDate = "CreateDate";
        private const string PickupActualDateTime = "PickupActualDateTime";
        private const string BookingConfirmationDate = "BookingConfirmationDate";
        private const string BookingConfirmationNumber = "BookingConfirmationNumber";
        private const string PickupEstimatedDateTime = "PickupEstimatedDateTime";
        private const string DepartureEstimationDate = "ETD";
        private const string DepartureDate = "ATD";
        private const string ArrivalEstimationDate = "ETA";
        private const string ArrivalDate = "ATA";





        public static List<FieldMap> fieldsMap = new List<FieldMap>()
        {
            new FieldMap("Tenant", "Tenant"),
            new FieldMap("EntityId", "Id"),
            new FieldMap("ForwardingShipmentHeaderId", "ShipmentId"),
            new FieldMap("CustomerId", "CustomerId"),
            new FieldMap("TransportModeId", "TransportModeId"),
            new FieldMap("Master", "Master"),
            new FieldMap("House", "House"),
            new FieldMap("ShipmentNumber", "OrderNumber"),
            new FieldMap("FromPortId", "OriginPortId"),
            new FieldMap("ToPortId", "DestinationPortId"),
            new FieldMap("ShipperId", "ShipperId"),
            new FieldMap("ConsigneeId", "ConsigneeId"),
            new FieldMap("CreateDate", CreateDate),
            new FieldMap("SecurityKey", "SecurityKey"),
            new FieldMap("ShipperName", "CasualImporterName"),
            new FieldMap("CustomerReference", "CustomerReferences"),
            new FieldMap("DirectionId", "DirectionId"),
            new FieldMap("ShipmentLevelCode", "ShipmentLevelCode"),
            new FieldMap("PickupDate", PickupActualDateTime),
            new FieldMap("BookingDate", BookingConfirmationDate),
            new FieldMap("PickupEstimationDate", PickupEstimatedDateTime),
            new FieldMap("DepartureEstimationDate", DepartureEstimationDate),
            new FieldMap("DepartureDate", DepartureDate),
            new FieldMap("ArrivalEstimationDate", ArrivalEstimationDate),
            new FieldMap("ArrivalDate", ArrivalDate),
            new FieldMap("PoNumber", "PoNumber"),
            new FieldMap("FromWarehouseDate", "OnHandDate"),
            new FieldMap("FromWarehouseNotes", "OnHandNumber"),
            new FieldMap("BookingNotes", "BookingConfirmationNumber"),

        };


        public static void SetTableLogic(SetTableLogicArgs args)
        {
            SetFixedValueFields(args.TableRow);
            MapTableFields(args);
            SetMilestonesDoneFields(args.TableRow);
            SetShipmentTypeCode(args.TableRow);
            SetMainEntity(args.TableRow);
            SetPreviousForwardingShipmentHeader(args.TableRow);
            SetCurrentMilestone(args);
            SetExceptionDescription(args.TableRow);
        }

        private static void SetFixedValueFields(DataRow tableRow)
        {
            tableRow.SetField("EntityType", ShipmentOrderEntityType);
        }

        private static void SetMilestonesDoneFields(DataRow tableRow)
        {
            tableRow.SetField("PickupDone", !IsFieldNullOrEmpty(tableRow, "PickupDate"));
            tableRow.SetField("BookingDone", !IsFieldNullOrEmpty(tableRow, "BookingDate"));
            tableRow.SetField("CreateDone", !IsFieldNullOrEmpty(tableRow, "CreateDate"));
            tableRow.SetField("DepartureDone", !IsFieldNullOrEmpty(tableRow, "DepartureDate"));
            tableRow.SetField("ArrivalDone", !IsFieldNullOrEmpty(tableRow, "ArrivalDate"));
            tableRow.SetField("FromWarehouseDone", !IsFieldNullOrEmpty(tableRow, "FromWarehouseDate"));


        }
        private static void SetMainEntity(DataRow tableRow)
        {
            tableRow.SetField("IsMainRecord", IsFieldNullOrEmpty(tableRow, "ForwardingShipmentHeaderId"));
        }
        private static void SetCurrentMilestone(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            var currentMilestoneArgs = new CheckCurrentMilestoneArgs(tableRow);
            foreach (var milestone in args.Milestones)
            {
                currentMilestoneArgs.milestone = milestone;
                CheckCurrentMilestone(currentMilestoneArgs, args);
                
            }


        }

        private static void CheckCurrentMilestone(CheckCurrentMilestoneArgs currentMilestoneArgs, SetTableLogicArgs args)
        {
            var tenant = (int)currentMilestoneArgs.tableRow["Tenant"];
            if (!CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, currentMilestoneArgs.milestone.Code, tenant))
                return;

            switch (currentMilestoneArgs.milestone.Code)
            {
                case CargoTrackingMilestoneValues.Created:
                    if (!IsFieldNullOrEmpty(currentMilestoneArgs.tableRow, "CreateDone") && !currentMilestoneArgs.tableRow["CreatedDone"].Equals("False"))
                    {
                        currentMilestoneArgs.date = currentMilestoneArgs.tableRow["CreateDate"];
                        CheckMilestone(currentMilestoneArgs);
                    }
                    break;
                case CargoTrackingMilestoneValues.Booking:
                    if (!IsFieldNullOrEmpty(currentMilestoneArgs.tableRow, "BookingDone") && !currentMilestoneArgs.tableRow["BookingDone"].Equals("False"))
                    {
                        currentMilestoneArgs.date = currentMilestoneArgs.tableRow["BookingDate"];
                        CheckMilestone(currentMilestoneArgs);
                    }
                    break;
                case CargoTrackingMilestoneValues.Pickup:
                    if (!IsFieldNullOrEmpty(currentMilestoneArgs.tableRow, "PickupDone") && !currentMilestoneArgs.tableRow["PickupDone"].Equals("False"))
                    {
                        currentMilestoneArgs.date = currentMilestoneArgs.tableRow["PickupDate"];
                        CheckMilestone(currentMilestoneArgs);
                    }
                    break;
                case CargoTrackingMilestoneValues.FromWarehouse:
                    if (!IsFieldNullOrEmpty(currentMilestoneArgs.tableRow, "FromWarehouseDone") && !currentMilestoneArgs.tableRow["FromWarehouseDone"].Equals("False"))
                    {
                        currentMilestoneArgs.date = currentMilestoneArgs.tableRow["FromWarehouseDate"];
                        CheckMilestone(currentMilestoneArgs);
                    }
                    break;
                case CargoTrackingMilestoneValues.Departure:
                    if (!IsFieldNullOrEmpty(currentMilestoneArgs.tableRow, "DepartureDone") && !currentMilestoneArgs.tableRow["DepartureDone"].Equals("False"))
                    {
                        currentMilestoneArgs.date = currentMilestoneArgs.tableRow["DepartureDate"];
                        CheckMilestone(currentMilestoneArgs);
                    }
                    break;
                case CargoTrackingMilestoneValues.Arrival:
                    if (!IsFieldNullOrEmpty(currentMilestoneArgs.tableRow, "ArrivalDone") && !currentMilestoneArgs.tableRow["ArrivalDone"].Equals("False"))
                    {
                        currentMilestoneArgs.date = currentMilestoneArgs.tableRow["ArrivalDate"];
                        CheckMilestone(currentMilestoneArgs);
                    }
                    break;

                default:
                    break;
            }
        }

        private static void SetExceptionDescription(DataRow tableRow)
        {
            var exceptionDate = !IsFieldNullOrEmpty(tableRow, "LastExceptionDate") ? tableRow["LastExceptionDate"]?.ToString() : null;
            var exceptionDescription = !IsFieldNullOrEmpty(tableRow, "LastExceptionDescription") ? "," + tableRow["LastExceptionDescription"] : null;
            tableRow.SetField("CurrentMilestoneExceptions", string.IsNullOrWhiteSpace(exceptionDate) ? exceptionDescription : exceptionDate + "," + tableRow["LastExceptionDescription"]);
        }
        private static void SetPreviousForwardingShipmentHeader(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "ForwardingShipmentHeaderId"))
                tableRow.SetField("PrevForwardingShipmentId", tableRow["ForwardingShipmentHeaderId"]);
        }
        private static void SetShipmentTypeCode(DataRow tableRow)
        {
            var transportMode = (string)tableRow["TransportModeId"];
            tableRow.SetField("ShipmentTypeCode", GetShipmentTypeCodeByTransportMode(transportMode));
        }

        private static string GetShipmentTypeCodeByTransportMode(string transportMode)
        {
            switch (transportMode)
            {
                case InlandTransportMode: return InlandShipmentType;
                case OceanTransportMode: return OceanShipmentType;
                case AirTransportMode: return AirShipmentType;
            }
            return null;
        }

        private static void MapTableFields(SetTableLogicArgs args)
        {
            var tableRow = args.TableRow;
            foreach (var field in fieldsMap)
            {
                SetField(field, tableRow, args);


            }
        }

        private static void SetField(FieldMap field, DataRow tableRow, SetTableLogicArgs args)
        {
            switch (field.OriginalFieldName)
            {
                case CreateDate:
                    SetCreateDate(field, tableRow, args);
                    break;
                case PickupActualDateTime:
                case PickupEstimatedDateTime:
                    SetPickupDates(field, tableRow, args);
                    break;
                case BookingConfirmationDate:
                    SetBookingConfirmationDate(field, tableRow, args);
                    break;
                case BookingConfirmationNumber:
                    SetBookingNotes(field, tableRow, args);
                    break;
                case DepartureDate:
                case DepartureEstimationDate:
                    SetDepartureDates(field, tableRow, args);
                    break;
                case ArrivalDate:
                case ArrivalEstimationDate:
                    SetArrivalDates(field, tableRow, args);
                    break;
                default:
                    tableRow.SetField(field.CargoTrackingFieldName, tableRow[field.OriginalFieldName]);
                    break;
            }
        }

        private static void SetBookingNotes(FieldMap field, DataRow tableRow, SetTableLogicArgs args)
        {
            var tenant = (int)tableRow["Tenant"];
            if (CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Booking, tenant))
                tableRow.SetField(field.CargoTrackingFieldName, tableRow[field.OriginalFieldName]);
            else
                tableRow.SetField(field.CargoTrackingFieldName, (DBNull)null);
        }

        private static void SetArrivalDates(FieldMap field, DataRow tableRow, SetTableLogicArgs args)
        {
            var tenant = (int)tableRow["Tenant"];
            if (CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Arrival, tenant))
                tableRow.SetField(field.CargoTrackingFieldName, tableRow[field.OriginalFieldName]);
            else
                tableRow.SetField(field.CargoTrackingFieldName, (DBNull)null);
        }

        private static void SetDepartureDates(FieldMap field, DataRow tableRow, SetTableLogicArgs args)
        {
            var tenant = (int)tableRow["Tenant"];
            if (CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Departure, tenant))
                tableRow.SetField(field.CargoTrackingFieldName, tableRow[field.OriginalFieldName]);
            else
                tableRow.SetField(field.CargoTrackingFieldName, (DBNull)null);
        }

        private static void SetBookingConfirmationDate(FieldMap field, DataRow tableRow, SetTableLogicArgs args)
        {
            var tenant = (int)tableRow["Tenant"];
            if (CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Booking, tenant))
                tableRow.SetField(field.CargoTrackingFieldName, "Booking Conf. Num: "+ tableRow[field.OriginalFieldName]);
            else
                tableRow.SetField(field.CargoTrackingFieldName, (DBNull)null);
        }

        private static void SetPickupDates(FieldMap field, DataRow tableRow, SetTableLogicArgs args)
        {
            var tenant = (int)tableRow["Tenant"];
            if (CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Pickup, tenant))
                tableRow.SetField(field.CargoTrackingFieldName, tableRow[field.OriginalFieldName]);
            else
                tableRow.SetField(field.CargoTrackingFieldName, (DBNull)null);
        }

        private static void SetCreateDate(FieldMap field, DataRow tableRow, SetTableLogicArgs args)
        {
            var tenant = (int)tableRow["Tenant"];
            if (CheckIfUserHasAccessToMilestone(args.NotPermittedMilestones, CargoTrackingMilestoneValues.Created, tenant))
                tableRow.SetField(field.CargoTrackingFieldName, tableRow[field.OriginalFieldName]);
            else
                tableRow.SetField(field.CargoTrackingFieldName, (DBNull)null);

        }

        private void AddFullFields()
        {
            // Booking - missing in cargo tracking
            //new FieldMap("BookingDone", "PickupDone"), 
            //new FieldMap("BookingDate", "BookingConfirmationDate"),            
            //new FieldMap("ConsigneeName", "ConsigneeName"), // not found
            //new FieldMap("CustomsShipmentHeaderId", "CustomsShipmentHeaderId"),
            //new FieldMap("CurrentMilestoneCode", "CurrentMilestoneCode"),
            //new FieldMap("CurrentMilestoneDate", "CurrentMilestoneDate"),
            //new FieldMap("GrossWeight", "GrossWeight"),
            //new FieldMap("Volume", "Volume"),
            //new FieldMap("IsMainRecord", "IsMainRecord"),
            //new FieldMap("FirstPickupETD", "FirstPickupETD"),
            //new FieldMap("DeclarationDate", "DeclarationDate"),
            //new FieldMap("CustomsClearanceDate", "CustomsClearanceDate"),            
            //new FieldMap("ContainersNumbers", "ContainersNumbers"),
            //new FieldMap("PackagesQuantity", "PackagesQuantity"),
            //new FieldMap("GrossWeightUnitCode", "GrossWeightUnitCode"),
            //new FieldMap("ForwardingShipmentNumber", "ForwardingShipmentNumber"),
            //new FieldMap("CurrentMilestoneExceptions", "CurrentMilestoneExceptions"),
            //new FieldMap("ForwardingHouse", "ForwardingHouse"),
            //new FieldMap("ForwardingMaster", "ForwardingMaster"),
            //new FieldMap("ForwardingShipmentLevelCode", "ForwardingShipmentLevelCode"),
            //new FieldMap("ImportManifest", "ImportManifest")

            //// FromWarehouse x
            //new FieldMap("FromWarehouseDate", "FromWarehouseDate"),
            //new FieldMap("FromWarehouseEstimationDate", "FromWarehouseEstimationDate"),
            //new FieldMap("FromWarehouseNotes", "FromWarehouseNotes"),
            //new FieldMap("FromWarehouseDone", "FromWarehouseDone"),

            //// Departure x
            //new FieldMap("DepartureDone", "DepartureDone"),
            //new FieldMap("DepartureDate", "DepartureDate"),
            //new FieldMap("DepartureEstimationDate", "DepartureEstimationDate"),

            //// Arrival x
            //new FieldMap("ArrivalDone", "ArrivalDone"),
            //new FieldMap("ArrivalDate", "ArrivalDate"),
            //new FieldMap("ArrivalEstimationDate", "ArrivalEstimationDate"),

            //// ToWarehouse x
            //new FieldMap("ToWarehouseDone", "ToWarehouseDone"),
            //new FieldMap("ToWarehouseDate", "ToWarehouseDate"),
            //new FieldMap("ToWarehouseEstimationDate", "ToWarehouseEstimationDate"),
            //new FieldMap("ToWarehouseNotes", "ToWarehouseNotes"),

            //// CustomsPayment x
            //new FieldMap("CustomsPaymentDone", "CustomsPaymentDone"),
            //new FieldMap("CustomsPaymentDate", "CustomsPaymentDate"),

            //// Clearance x
            //new FieldMap("ClearanceDone", "ClearanceDone"),
            //new FieldMap("ClearanceDate", "ClearanceDate"),

            //// Delivered x
            //new FieldMap("DeliveredDone", "DeliveredDone"),
            //new FieldMap("DeliveredDate", "DeliveredDate"),
            //new FieldMap("DeliveredEstimationDate", "DeliveredEstimationDate"),

            //// WarehouseLeg x
            //new FieldMap("WarehouseLegActualEntryDate", "WarehouseLegActualEntryDate"),
            //new FieldMap("WarehouseLegExpectedEntryDate", "WarehouseLegExpectedEntryDate"),
            //new FieldMap("WarehouseLegRemarks", "WarehouseLegRemarks"),

            //// AssignedTrucker x
            //new FieldMap("AssignedTruckerDone", "AssignedTruckerDone"),
            //new FieldMap("AssignedTruckerDate", "AssignedTruckerDate"),
            //new FieldMap("AssignedTruckerEstimationDate", "AssignedTruckerEstimationDate"),
            //new FieldMap("AssignedTruckerNotes", "AssignedTruckerNotes"),

            //// AssignedCustomsAgent x
            //new FieldMap("AssignedCustomsAgentDone", "AssignedCustomsAgentDone"),
            //new FieldMap("AssignedCustomsAgentDate", "AssignedCustomsAgentDate"),
            //new FieldMap("AssignedCustomsAgentEstDate", "AssignedCustomsAgentEstDate"),
            //new FieldMap("AssignedCustomsAgentNotes", "AssignedCustomsAgentNotes"),
            //new FieldMap("AssignedCustomsAgentExcReason", "AssignedCustomsAgentExcReason"),

            //// Delivery x
            //new FieldMap("DeliveryDone", "DeliveryDone"),
            //new FieldMap("DeliveryDate", "DeliveryDate"),
            //new FieldMap("DeliveryEstimationDate", "DeliveryEstimationDate"),
            //new FieldMap("DeliveryNotes", "DeliveryNotes"),
            //new FieldMap("DeliveryExceptionReason", "DeliveryExceptionReason"),


            //// GoodsClassification x
            //new FieldMap("GoodsClassificationDate", "GoodsClassificationDate"),
            //new FieldMap("GoodsClassificationEstDate", "GoodsClassificationEstDate"),
            //new FieldMap("GoodsClassificationNotes", "GoodsClassificationNotes"),
            //new FieldMap("GoodsClassificationDone", "GoodsClassificationDone"),

            //// DocumentInspection x
            //new FieldMap("DocumentInspectionDate", "DocumentInspectionDate"),
            //new FieldMap("DocumentInspectionEstDate", "DocumentInspectionEstDate"),
            //new FieldMap("DocumentInspectionNotes", "DocumentInspectionNotes"),
            //new FieldMap("DocumentInspectionDone", "DocumentInspectionDone"),

            //// GatepassArrived x
            //new FieldMap("GatepassArrivedDate", "GatepassArrivedDate"),
            //new FieldMap("GatepassArrivedEstDate", "GatepassArrivedEstDate"),
            //new FieldMap("GatepassArrivedNotes", "GatepassArrivedNotes"),
            //new FieldMap("GatepassArrivedDone", "GatepassArrivedDone"),

        }
        private static bool IsFieldNullOrEmpty(DataRow tableRow, string coulmnName)
        {
            if (tableRow[coulmnName].Equals(null) || tableRow[coulmnName].Equals(0) || tableRow[coulmnName].Equals("") || tableRow[coulmnName].GetType().Name == "DBNull")
                return true;
            return false;
        }
    }

    public class FieldMap
    {
        public FieldMap(string cargoTrackingFieldName, string originalFieldName)
        {
            CargoTrackingFieldName = cargoTrackingFieldName;
            OriginalFieldName = originalFieldName;
        }
        public string CargoTrackingFieldName { get; set; }
        public string OriginalFieldName { get; set; }
    }
}
