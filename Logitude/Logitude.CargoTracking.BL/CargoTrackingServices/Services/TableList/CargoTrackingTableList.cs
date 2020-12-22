using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public static class CargoTrackingTableList
    {
        public static List<CargoTable> FillCargoTableList()
        {
            List<CargoTable> CargoTableLists = new List<CargoTable>();

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Port",
                FieldsDBName = "Id,Tenant,Code,CountryId,EnglishName,AutomaticLastUpdateDate",
                CT_FieldsDBName = "Id,Code,EnglishName,CountryId,Tenant",
                KeyName = "Id",
                ConditionKey = "Id",
                DBTableName = "Ports",
                CT_TableName = "CargoTrackingPorts",
                Main_CT_TableName = "CargoTrackingPorts",
                Pre_TableName = "Pre_CargoTrackingPorts",
                ObjectTableName = "CargoTrackingPort",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Card",
                FieldsDBName = "Id,Tenant,Code,LocalName,EnglishName,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Code,EnglishName,LocalName,Tenant",
                DBTableName = "Cards",
                CT_TableName = "CargoTrackingCards",
                Main_CT_TableName = "CargoTrackingCards",
                Pre_TableName = "Pre_CargoTrackingCards",
                ObjectTableName = "CargoTrackingCard",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "TransportModes",
                FieldsDBName = "Id,Name,SearchFields,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Name,SearchFields",
                DBTableName = "TransportModes",
                CT_TableName = "CargoTrackingTransportModes",
                Main_CT_TableName = "CargoTrackingTransportModes",
                Pre_TableName = "Pre_CargoTrackingTransportModes",
                ObjectTableName = "CargoTrackingTransportMode",
                ConditionsNumber = 1,
                IsClosedTable = true,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Countries",
                FieldsDBName = "Id,LocalName,Code,EnglishName,Tenant,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,LocalName,Code,EnglishName,Tenant",
                DBTableName = "Countries",
                CT_TableName = "CargoTrackingCountries",
                Main_CT_TableName = "CargoTrackingCountries",
                Pre_TableName = "Pre_CargoTrackingCountries",
                ObjectTableName = "CargoTrackingCountry",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "ShipmentMasterDatas",
                FieldsDBName = "Id,Master,MainCarriageATD,MainCarriageETD,MainCarriageATA,MainCarriageETA,Tenant,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Tenant,Master,MainCarriageATD,MainCarriageETD,MainCarriageATA,MainCarriageETA",
                DBTableName = "ShipmentMasterDatas",
                CT_TableName = "CargoTrackingShipmentMasters",
                Main_CT_TableName = "CargoTrackingShipmentMasters",
                Pre_TableName = "Pre_CargoTrackingShipmentMasters",
                ObjectTableName = "CargoTrackingShipmentMaster",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "ShipmentComputedFields",
                FieldsDBName = "Id,FirstPickupATD,FinalDeliveryATA,FinalDeliveryETA,Tenant,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,FirstPickupATD,FinalDeliveryATA,Tenant,FinalDeliveryETA",
                DBTableName = "ShipmentComputedFields",
                CT_TableName = "CargoTrackingShipmentComputeds",
                Main_CT_TableName = "CargoTrackingShipmentComputeds",
                Pre_TableName = "Pre_CargoTrackingShipmentComputeds",
                ObjectTableName = "CargoTrackingShipmentComputed",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Shipment",
                FieldsDBName = "Id,Tenant,CustomFileNumber,ForwarderShipmentNumber,CustomsDeclarationNumber,ShipperName,CustomerId,TransportModeId,DirectionId,MasterShipmentDataId,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,CustomConnectToShipment,AutomaticLastUpdateDate,ShipmentPickUpIndex,FirstPickupETA,ShipmentLevelCode,CustomsClearanceDate,CustomFileId,CreateDateTime,SecurityKey,ConsigneeName,CustomerReference1,CustomerReference2,FirstPickupETD,WarehouseLegActualEntryDate,WarehouseLegExpectedEntryDate,WarehouseLegRemarks,DeclarationDate,IsCancelled,SearchFields,PackagesQuantity",
                KeyName = "Id",
                KeyName2 = "Id",
                ConditionKey = "EntityId",
                ConditionKey2 = "ShipmentId",
                DBTableName = "Shipments",
                CT_TableName = "CargoTrackingShipments",
                CT2_TableName = "CargoTrackingShipmentSearches",
                Main_CT_TableName = "CargoTrackingShipments",
                Main_CT2_TableName = "CargoTrackingShipmentSearches",
                CT_FieldsDBName = "Tenant,ShipperName,IsMainRecord,CustomerId,TransportModeId,DirectionId,Master,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,PickupDone,PickupDate,PickupEstimationDate,FromWarehouseDone,FromWarehouseNotes,DepartureDate,DepartureEstimationDate,ClearanceDone,ClearanceDate,FromWarehouseEstimationDate,FromWarehouseDate,DepartureDone,ArrivalDone,ToWarehouseDate,ToWarehouseEstimationDate,DeliveredEstimationDate,DeliveredDone,DeliveredDate,ArrivalEstimationDate,ToWarehouseDone,ArrivalDate,EntityId,EntityType,ForwardingShipmentHeaderId,CustomsShipmentHeaderId,CurrentMilestoneCode,CurrentMilestoneDate,ToWarehouseNotes,CustomsPaymentDone,CustomsPaymentDate,CreateDate,SecurityKey,ConsigneeName,CustomerReference,ContainersNumbers,PackagesQuantity",
                CT2_FieldsDBName = "Tenant,ShipmentId,SearchFields,ShipmentDate,IsPublic",
                Condition1 = " ((ShipmentLevelCode ='D' or ShipmentLevelCode ='H') and CustomFileId is not null)",
                Condition2 = " ((ShipmentLevelCode !='D' and ShipmentLevelCode !='H') or CustomFileId is null)",
                Pre_TableName = "Pre_CargoTrackingShipments",
                Pre2_TableName = "Pre_CargoTrackingShipmentSearches",
                ObjectTableName = "CargoTrackingShipment",
                InnerObjectTableName = "CargoTrackingShipmentSearch",
                ConditionsNumber = 2,
            });





            return CargoTableLists;

        }
    }
}
