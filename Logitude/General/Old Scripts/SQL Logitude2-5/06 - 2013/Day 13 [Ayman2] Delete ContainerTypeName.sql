

delete from ObjectFields where FieldName = 'ContainerTypeName' and ObjectTableId = (Select Id from ObjectTables where name = 'ShipmentPackage')
go

delete from TextCodes where Code like '%ContainerTypeName%' and ObjectTableId = (Select Id from ObjectTables where name = 'ShipmentPackage')
go
