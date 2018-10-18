
delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'StatusCode' and ObjectTableId = (select Id from ObjectTables where Name = 'WarehouseEntry'))
delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'StatusCode' and ObjectTableId = (select Id from ObjectTables where Name = 'WarehouseRelease'))
