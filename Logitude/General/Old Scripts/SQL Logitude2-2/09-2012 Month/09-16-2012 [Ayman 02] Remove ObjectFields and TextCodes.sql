

--delete from ObjectFields where FieldName = 'RateClassCode' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentPackage')
--delete from ObjectFields where FieldName = 'AWBRateCharge' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentPackage')
--delete from ObjectFields where FieldName = 'CommodityNumber' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentPackage')
--delete from ObjectFields where FieldName = 'FreightAmount' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentPackage')
--delete from ObjectFields where FieldName = 'ChargeableWeight' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentPackage')

--delete from TextCodes where Code = 'ShipmentPackage.F.RateClassCode'
--delete from TextCodes where Code = 'ShipmentPackage.F.AWBRateCharge'
--delete from TextCodes where Code = 'ShipmentPackage.F.CommodityNumber'
--delete from TextCodes where Code = 'ShipmentPackage.F.FreightAmount'
--delete from TextCodes where Code = 'ShipmentPackage.F.ChargeableWeight'

--delete from TextCodes where Code = 'ShipmentPackage.RateClassCodeHelpText'
--delete from TextCodes where Code = 'ShipmentPackage.AWBRateChargeHelpText'
--delete from TextCodes where Code = 'ShipmentPackage.CommodityNumberHelpText'
--delete from TextCodes where Code = 'ShipmentPackage.FreightAmountHelpText'
--delete from TextCodes where Code = 'ShipmentPackage.ChargeableWeightHelpText'


