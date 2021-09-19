using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure
{
    public class CargoTrackingShipmentTableStructure
    {
        public List<string> columnsForCopy = new List<string>
            {
                "Id",
                "Tenant",
                "CustomFileNumber",
                "ForwarderShipmentNumber",
                "CustomsDeclarationNumber",
                "ShipperName",
                "CustomerId",
                "TransportModeId",
                "DirectionId",
                "MasterShipmentDataId",
                "House",
                "ShipmentNumber",
                "FromPortId",
                "ToPortId",
                "ShipperId",
                "ConsigneeId",
                "GrossWeight",
                "Volume",
                "CustomConnectToShipment",
                "AutomaticLastUpdateDate",
                "ShipmentPickUpIndex",
                "FirstPickupETA",
                "ShipmentLevelCode",
                "CustomsClearanceDate",
                "CustomFileId",
                "CreateDateTime",
                "SecurityKey",
                "ConsigneeName",
                "CustomerReference1",
                "CustomerReference2",
                "FirstPickupETD",
                "WarehouseLegActualEntryDate",
                "WarehouseLegExpectedEntryDate",
                "WarehouseLegRemarks",
                "DeclarationDate",
                "IsCancelled",
                "SearchFields",
                "PackagesQuantity",

                "AssignedToTruckerDate",
                "AssginedToCustomsAgentDate",
                "GrossWeightUnitCode",
                "ShipmentTypeId",

                "ExceptionDate",
                "ExceptionDescription"
            };
        public List<string> fields = new List<string>
            {
                "Id",
                "Tenant",
                "CreateDate",
                "CreatedByUserId",
                "UpdateDate",
                "UpdatedByUserId",
                "SearchFields",
                "OrderNumber",
                "TransportModeId",
                "ConsigneeId",
                "ShipperId",
                "AgentId",
                "IncotermId",
                "AccountManagerId",
                "PONumber",
                "DescriptionOfGoods",
                "Master",
                "House",
                "CarrierNumber",
                "VesselId",
                "ETD",
                "ETA",
                "ATD",
                "ATA",
                "CustomsAgentId",
                "SpecialServicesTypeId",
                "CustomerReferences",
                "IsReadyForPickup",
                "PickupEstimatedDateTime",
                "PickupActualDateTime",
                "ForwarderId",
                "BookingConfirmationDate",
                "ShipmentNumber",
                "SupplyDateTime",
                "OriginPortId",
                "DestinationPortId",
                "GatewayId",
                "CasualImporterName",
                "CasualSupplierName",
                "ShipmentLevelCode",
                "PODate",
                "BookingConfirmationNumber",
                "DirectionId",
                "CarrierId",
                "IsCancelled",
                "SecurityKey",
                "ShipmentId",
                "Quantity",
                "GrossWeight",
                "Volume",
                "CustomerId"
            };
        public string GetColumnsForCopy()
        {
            return string.Join(",", columnsForCopy.ToArray());
        }

        public string GetDummyColumnsForCopy()
        {
            List<string> ColumnsForCopy = new List<string>();
            ColumnsForCopy.Add("IsPublic");
            ColumnsForCopy.Add("ReferenceType");

            return string.Join(",", ColumnsForCopy.ToArray());
        }
        public string GetShipmentOrderFields()
        {
            

            return string.Join(",", fields.ToArray());
        }

    }
}
