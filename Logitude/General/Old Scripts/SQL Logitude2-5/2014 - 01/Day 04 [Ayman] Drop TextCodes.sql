

delete from ObjectFields where fieldname ='entityId' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

delete from ObjectFields where fieldname ='ObjectTableId' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

delete from TextCodes where Code like '%Activity%entityId%' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

delete from TextCodes where Code like '%Activity%ObjectTableId%' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

