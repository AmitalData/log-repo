delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'IsBlocked' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'BlockDate' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'BlockNote' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PlimusAccount' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement'))

delete from ObjectFields where FieldName = 'IsBlocked' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
delete from ObjectFields where FieldName = 'BlockDate' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
delete from ObjectFields where FieldName = 'BlockNote' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
delete from ObjectFields where FieldName = 'PlimusAccount' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')

delete from TextCodes where Code like '%IsBlocked%' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
delete from TextCodes where Code like '%BlockDate%' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
delete from TextCodes where Code like '%BlockNote%' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
delete from TextCodes where Code like '%PlimusAccount%' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')

