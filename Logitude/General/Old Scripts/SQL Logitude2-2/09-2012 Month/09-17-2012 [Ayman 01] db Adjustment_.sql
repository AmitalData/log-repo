

alter table ShipmentPackages drop [FK_ShipmentCommodityShipmentPackage]
drop INDEX ShipmentPackages.[IX_FK_ShipmentCommodityShipmentPackage]
alter table ShipmentPackages drop column ShipmentCommodityId
alter table ShipmentPackages drop column RateClassCode

alter table ShipmentCommodities drop [FK_ShipmentShipmentCommodity]
drop INDEX ShipmentCommodities.[IX_FK_ShipmentShipmentCommodity]

alter table ShipmentCommodities drop [FK_ShipmentCommodityRateClass]
drop INDEX ShipmentCommodities.[IX_FK_ShipmentCommodityRateClass]

drop table ShipmentCommodities

delete from ObjectFields where FieldName = 'RateClassCode' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')
delete from ObjectFields where FieldName = 'ChargeRate' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')
delete from ObjectFields where FieldName = 'CommodityNumber' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')
delete from ObjectFields where FieldName = 'ChargeAmount' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')
delete from ObjectFields where FieldName = 'ChargeableWeight' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')

delete from TextCodes where Code = 'ShipmentCommodity.F.RateClassCode'
delete from TextCodes where Code = 'ShipmentCommodity.F.ChargeRate'
delete from TextCodes where Code = 'ShipmentCommodity.F.CommodityNumber'
delete from TextCodes where Code = 'ShipmentCommodity.F.ChargeAmount'
delete from TextCodes where Code = 'ShipmentCommodity.F.ChargeableWeight'

delete from TextCodes where Code = 'ShipmentCommodity.RateClassCodeHelpText'
delete from TextCodes where Code = 'ShipmentCommodity.ChargeRateHelpText'
delete from TextCodes where Code = 'ShipmentCommodity.CommodityNumberHelpText'
delete from TextCodes where Code = 'ShipmentCommodity.ChargeAmountHelpText'
delete from TextCodes where Code = 'ShipmentCommodity.ChargeableWeightHelpText'

delete from TextCodes where Code = 'ShipmentCommodity'
delete from ObjectTables where Name = 'ShipmentCommodity'