
delete 
from	ObjectFields 
where	ObjectTableId = ( select Id from ObjectTables where Name = 'GLAccount')
 and	(FieldName like '%client%' or  FieldName like 'vendor%')
