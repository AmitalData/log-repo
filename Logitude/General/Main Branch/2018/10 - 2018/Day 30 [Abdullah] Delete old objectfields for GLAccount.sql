
delete 
from	ObjectFields 
where	ObjectTableId = ( select Id from ObjectTables where Name = 'GLAccount')
 and	(FieldName like '%client%' or  FieldName like 'vendor%')



 
--USAGE: Delete ObjectTable Queries Query
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'glaccount'))
delete from AdvancedQueryFilters where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'glaccount'))
delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'glaccount')
