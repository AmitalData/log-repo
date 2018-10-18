

-- Run this SQL then update Tenants

delete from AdvancedQueryFilters where QueryId = (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'CustomerSize'))
go

delete from QueryColumns where QueryId = (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'CustomerSize'))
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'CustomerSize'))
go

delete from ObjectFields where FieldName = 'Rank' and ObjectTableId = (select Id from ObjectTables where Name = 'CustomerSize')
go

delete from TextCodes where Code like '%CustomerSize%Rank%'
go
