
select  MasterShipmentDataId,ShipmentLevelCode from Shipments
 where MasterShipmentDataId is not null  and datalength(MasterShipmentDataId)<>0 and  MasterShipmentDataId not in (select id from ShipmentMasterDatas)

update Shipments set MasterShipmentDataId = null
where MasterShipmentDataId is not null  and datalength(MasterShipmentDataId)<>0 and  MasterShipmentDataId not in (select id from ShipmentMasterDatas)


update Shipments set MasterShipmentDataId = null where MasterShipmentDataId is not null and datalength(MasterShipmentDataId)=0