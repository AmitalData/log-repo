using Logitude.CargoTracking.BL.CloseTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class CargoTrackingShipmentsOrderLogicService
    {
        private const string InlandTransportMode = "I";
        private const string OceanTransportMode = "O";
        private const string AirTransportMode = "A";
        private const string InlandShipmentType = "FTL";
        private const string OceanShipmentType = "FCLD";
        private const string AirShipmentType = "AIR";
        private const string ShipmentOrderEntityType = "O";
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
            new FieldMap("CreateDate", "CreateDate"),
            new FieldMap("SecurityKey", "SecurityKey"),
            new FieldMap("ShipperName", "CasualImporterName"),
            new FieldMap("CustomerReference", "CustomerReferences"),
            new FieldMap("DirectionId", "DirectionId"),
            new FieldMap("ShipmentLevelCode", "ShipmentLevelCode"),
            new FieldMap("PickupDate", "PickupActualDateTime"),
            new FieldMap("BookingDate", "BookingConfirmationDate"),
            new FieldMap("PickupEstimationDate", "PickupEstimatedDateTime"),
            
        };
        public static void SetTableLogic(DataRow tableRow)
        {
            SetFixedValueFields(tableRow);
            MapTableFields(tableRow);
            SetMilestonesDoneFields(tableRow);
            SetShipmentTypeCode(tableRow);
            SetMainEntity(tableRow);
            SetPreviousForwardingShipmentHeader(tableRow);
            SetCurrentMilestone(tableRow);
        }

        private static void SetFixedValueFields(DataRow tableRow)
        {
            tableRow.SetField("EntityType", ShipmentOrderEntityType);
        }

        private static void SetMilestonesDoneFields(DataRow tableRow)
        {
            tableRow.SetField("PickupDone", !IsFieldNullOrEmpty(tableRow, "PickupActualDateTime"));
            tableRow.SetField("BookingDone", !IsFieldNullOrEmpty(tableRow, "BookingConfirmationDate"));
        }
        private static void SetMainEntity(DataRow tableRow)
        {
            tableRow.SetField("IsMainRecord", IsFieldNullOrEmpty(tableRow, "ForwardingShipmentHeaderId"));
        }
        private static void SetCurrentMilestone(DataRow tableRow)
        {
            if (!IsFieldNullOrEmpty(tableRow, "PickupDone") && !tableRow["PickupDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Pickup);
                tableRow.SetField("CurrentMilestoneDate", tableRow["PickupDate"]);
            } 
            else if (!IsFieldNullOrEmpty(tableRow, "BookingDone") && !tableRow["BookingDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Booking);
                tableRow.SetField("CurrentMilestoneDate", tableRow["BookingDate"]);
            }
            else if (!IsFieldNullOrEmpty(tableRow, "CreateDone") && !tableRow["CreateDone"].Equals("False"))
            {
                tableRow.SetField("CurrentMilestoneCode", CargoTrackingMilestoneValues.Created);
                tableRow.SetField("CurrentMilestoneDate", tableRow["CreateDate"]);

            }

        }
        private static void SetPreviousForwardingShipmentHeader(DataRow tableRow)
        {
            if(!IsFieldNullOrEmpty(tableRow, "ForwardingShipmentHeaderId"))
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

        private static void MapTableFields(DataRow tableRow)
        {
            foreach (var field in fieldsMap)
            {
                tableRow.SetField(field.CargoTrackingFieldName, tableRow[field.OriginalFieldName]);
            }
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
            if (tableRow[coulmnName].Equals(null) || tableRow[coulmnName].Equals("") || tableRow[coulmnName].GetType().Name == "DBNull")
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
