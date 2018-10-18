--USAGE: delete object field
delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'customs.declaration') and FieldName  = 'ImporterNumber')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'customs.declaration') and FieldName  = 'ImporterNumber')
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'customs.declaration') and FieldName  = 'ImporterNumber'