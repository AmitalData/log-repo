

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'InActive' and ObjectTableId = (select Id from ObjectTables where Name = 'QuoteStage'))
go

delete from ObjectFields where FieldName = 'InActive' and ObjectTableId = (select Id from ObjectTables where Name = 'QuoteStage')
go

delete from TextCodes where Code like '%InActive%' and ObjectTableId = (select Id from ObjectTables where Name = 'QuoteStage')
go