
-- Run this SQL then update Tenants
delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'AWBMessagingStock'))
go
