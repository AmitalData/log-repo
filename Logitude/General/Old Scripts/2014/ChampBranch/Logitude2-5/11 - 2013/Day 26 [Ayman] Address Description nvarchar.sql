

alter table Addresses alter column Description nvarchar(60)
go

update ObjectFields set DataTypeCode = 'nText' where FieldName = 'Description' AND ObjectTableId = (select Id from ObjectTables where Name = 'Address')
go
