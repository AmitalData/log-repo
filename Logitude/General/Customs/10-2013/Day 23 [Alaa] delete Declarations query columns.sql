delete from QueryColumns where ObjectFieldId = ( select id from ObjectFields where FieldName = 'DeclarationOfficeCode' and ObjectTableId= ( select id from ObjectTables where Name= 'customs.declaration'))


delete from QueryColumns where ObjectFieldId = ( select id from ObjectFields where FieldName = 'DeclarationStatusTypeCode' and ObjectTableId= ( select id from ObjectTables where Name= 'customs.declaration'))


