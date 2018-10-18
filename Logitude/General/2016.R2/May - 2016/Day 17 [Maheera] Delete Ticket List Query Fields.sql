delete from QueryColumns 
where QueryId in (select Id from Queries where ObjectTableId = (select id from ObjectTables where name ='ticket')) 
and ObjectFieldId = (Select id from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='ticket') and FieldName = 'FirstResponseDue') 

delete from QueryColumns 
where QueryId in (select Id from Queries where ObjectTableId = (select id from ObjectTables where name ='ticket')) 
and ObjectFieldId = (Select id from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='ticket') and FieldName = 'ResolveWithinDue') 


delete from ScreenFields where ScreenId in (select id from screens where ObjectTableId = (select id from ObjectTables where name ='ticket'))
and ObjectFieldId = (select Id from ObjectFields where FieldName = 'FirstResponseDue') or ObjectFieldId = (select Id from ObjectFields where FieldName = 'ResolveWithinDue')