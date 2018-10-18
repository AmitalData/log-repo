
select ShipmentNumber,MasterShipmentDataId from Shipments where ShipmentLevelCode = 'D'  and MasterShipmentDataId is null

update Shipments set MasterShipmentDataId = Id where ShipmentLevelCode = 'D'  and MasterShipmentDataId is null