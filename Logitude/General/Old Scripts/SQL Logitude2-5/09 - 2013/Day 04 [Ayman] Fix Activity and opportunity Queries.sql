
-- 1) Run this Script
-- 2) Update CRM

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Activity'))
go

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
go
