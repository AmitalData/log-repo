
alter table Shipments alter column FNAReason varchar(256) null
go

update Objectfields set MaxLength = 256 where FieldName = 'FNAReason' and ObjectTableId = (select Id from ObjectTables where Name  = 'Shipment')
go