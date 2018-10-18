-- Do not run online 
update ShipmentReceivables set IATACodeCode = (Select IATACodeCode from ChargesTypes where Id = ChargesTypeId) where IATACodeCode is null
go

update ShipmentPayables set IATACodeCode = (Select IATACodeCode from ChargesTypes where Id = ChargesTypeId) where IATACodeCode is null
go

update ShipmentAWBPrintOnlies set IATACodeCode = (Select IATACodeCode from ChargesTypes where Id = ChargesTypeId) where IATACodeCode is null
go


alter table ShipmentAWBPrintOnlies drop FK_ShipmentAWBPrintOnlyChargesType
go

DROP INDEX IX_FK_ShipmentAWBPrintOnlyChargesType ON ShipmentAWBPrintOnlies
go

alter table ShipmentAWBPrintOnlies drop column ChargesTypeId
go

delete from ObjectFields where FieldName = 'ChargesTypeId' and ObjectTableId = (Select Id from ObjectTables where Name = 'ShipmentAWBPrintOnly')
go

delete from TextCodes where Code like '%ShipmentAWBPrintOnly%ChargesType%'
go

