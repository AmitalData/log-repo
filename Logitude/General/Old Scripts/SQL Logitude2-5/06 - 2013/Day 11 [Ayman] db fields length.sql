
alter table Tenants alter column Signature varchar(60)
go

alter table Shipments alter column AWBSignature varchar(60)
go

alter table Addresses alter column Description varchar(60)
go

alter table Customs.Consignments alter column CargoDescription nvarchar(250)
go

update ObjectFields set MaxLength = 60 where FieldName = 'Signature' and ObjectTableId = (Select Id from ObjectTables where Name = 'Tenant')
go

update ObjectFields set MaxLength = 60 where FieldName = 'AWBSignature' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')
go

update ObjectFields set MaxLength = 60 where FieldName = 'Description' and ObjectTableId = (Select Id from ObjectTables where Name = 'Address')
go

update ObjectFields set DataTypeCode = 'nText' where FieldName = 'CargoDescription' and ObjectTableId = (Select Id from ObjectTables where Name = 'Customs.Consignment')
go