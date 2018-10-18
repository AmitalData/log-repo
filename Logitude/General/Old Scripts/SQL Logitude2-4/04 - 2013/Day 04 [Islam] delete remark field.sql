 
delete from ObjectFields where FieldName = 'remark'  and ObjectTableId = (select id from ObjectTables where name = 'country') 
delete from ObjectFields where FieldName = 'remark'  and ObjectTableId = (select id from ObjectTables where name = 'incoterm') 
delete from ObjectFields where FieldName = 'remark'  and ObjectTableId = (select id from ObjectTables where name = 'state') 
delete from ObjectFields where FieldName = 'remark'  and ObjectTableId = (select id from ObjectTables where name = 'contact') 
delete from ObjectFields where FieldName = 'remark'  and ObjectTableId = (select id from ObjectTables where name = 'user') 