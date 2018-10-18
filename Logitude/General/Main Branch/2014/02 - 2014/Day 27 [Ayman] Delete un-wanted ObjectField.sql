
delete from ObjectFields where FieldName = 'Weight' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')
go

delete from TextCodes where Code = 'ShipmentCommodity.F.Weight' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')
go

delete from TextCodes where Code = 'ShipmentCommodity.WeightHelpText' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentCommodity')
go


