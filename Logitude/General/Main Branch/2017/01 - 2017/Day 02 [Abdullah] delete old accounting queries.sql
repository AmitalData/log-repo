
-- (Before Update Accounting) Execute this script to remove old queries:

-- SCRIPT : Delete old queries columns
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'bankaccount'))
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'bankdeposit'))
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'cashbook'))
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'glaccount'))
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'journal'))
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'chartofaccount'))

-- USAGE: Delete ObjectTable's Queries Query
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'glaccount'))
delete from AdvancedQueryFilters where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'glaccount'))
delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'GLAccount')

--
-- Execute and (Update Accounting)
--
