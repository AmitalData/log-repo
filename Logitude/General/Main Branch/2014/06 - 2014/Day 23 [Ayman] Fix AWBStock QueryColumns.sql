
-- Run this SQL then update Tenants
delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'AWBMessagingStock'))
go

delete from ObjectFields where FieldName = 'Tenant' and ObjectTableId = (select Id from ObjectTables where Name = 'AWBMessagingStock')
go

delete from TextCodes where Code like '%Tenant%' and ObjectTableId = (select Id from ObjectTables where Name = 'AWBMessagingStock')
go