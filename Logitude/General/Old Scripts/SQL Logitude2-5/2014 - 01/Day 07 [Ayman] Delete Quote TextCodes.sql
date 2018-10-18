

delete from ObjectFields where FieldName = 'RequesterId' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

delete from ObjectFields where FieldName = 'RequesterName' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

delete from ObjectFields where FieldName = 'PotentialRequesterId' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

delete from TextCodes where Code like '%Requester%' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go