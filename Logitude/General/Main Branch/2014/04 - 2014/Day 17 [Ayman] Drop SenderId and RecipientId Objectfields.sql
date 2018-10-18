
delete from ObjectFields where FieldName = 'SenderId' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

delete from ObjectFields where FieldName = 'RecipientId' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

delete from TextCodes where Code like '%SenderId%' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

delete from TextCodes where Code like '%RecipientId%' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go


