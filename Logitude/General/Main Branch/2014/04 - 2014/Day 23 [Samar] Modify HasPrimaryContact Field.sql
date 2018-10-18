delete from ObjectFields where FieldName = 'HasPrimaryContact' and ObjectTableId = (select Id from ObjectTables where Name = 'Tenant')
delete from TextCodes where Code like '%HasPrimaryContact%'