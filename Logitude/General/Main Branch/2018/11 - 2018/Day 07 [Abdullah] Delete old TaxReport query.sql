
--USAGE: Delete ObjectTable Queries Query
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'TaxReport'))
delete from AdvancedQueryFilters where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'TaxReport'))
delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'TaxReport')