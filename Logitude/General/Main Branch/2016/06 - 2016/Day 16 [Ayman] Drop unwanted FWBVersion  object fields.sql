
            delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'FWBVersion' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement'))
            delete from ObjectFields where FieldName = 'FWBVersion' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
            delete from TextCodes where Code like '%FWBVersion%' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')

            delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'FHLVersion' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement'))
            delete from ObjectFields where FieldName = 'FHLVersion' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
            delete from TextCodes where Code like '%FHLVersion%' and ObjectTableId = (select Id from ObjectTables where Name = 'TenantManagement')
