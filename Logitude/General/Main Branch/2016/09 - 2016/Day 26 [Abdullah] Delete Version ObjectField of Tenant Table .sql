delete from ObjectFields where FieldName = 'Version' and ObjectTableId = ( select Id from ObjectTables where Name = 'Tenant')
