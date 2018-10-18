
-- Run this SQL then run update tenants, update CRM

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
go

delete from ObjectFields where FieldName = 'Topic' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code like '%Topic%' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go