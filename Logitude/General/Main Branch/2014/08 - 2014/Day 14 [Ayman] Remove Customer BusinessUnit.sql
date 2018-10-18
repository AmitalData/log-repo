
delete from ObjectFields where FieldName = 'BusinessUnitId' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')
go

delete from TextCodes where Code like '%Business%' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')
go
