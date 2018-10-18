
delete from ObjectFields where FieldName = 'Id' and ObjectTableId = (Select Id from ObjectTables where Name = 'Customs.Declaration')
go

delete from ObjectFields where FieldName = 'Id' and ObjectTableId = (Select Id from ObjectTables where Name = 'Customs.Client')
go

delete from ObjectFields where FieldName = 'Id' and ObjectTableId = (Select Id from ObjectTables where Name = 'Customs.Vendor')
go

delete from TextCodes where Code like '%Customs.%.Id%'
go
