
-- Run this script then update tenants
delete from ScreenModifications where ScreenId in (select Id from Screens where ObjectTableId = ((select Id from ObjectTables where Name = 'AdditionalService')))
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = ((select Id from ObjectTables where Name = 'AdditionalService')))
go

update ObjectTables set HeaderScreenId = null where Name = 'AdditionalService'
go

delete from Screens where ObjectTableId = ((select Id from ObjectTables where Name = 'AdditionalService'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'AdditionalService'))
go

delete from ObjectFields where FieldName = 'Code' and ObjectTableId = (select Id from ObjectTables where Name = 'AdditionalService')
go

delete from TextCodes where Code like '%Code%' and ObjectTableId = (select Id from ObjectTables where Name = 'AdditionalService')
go

