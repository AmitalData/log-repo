
delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CountryId' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement'))
delete from ObjectFields where FieldName = 'CountryId' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
delete from TextCodes where Code like '%CountryId%' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')