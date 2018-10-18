
delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'By Last Shipment') and ObjectFieldId = (select Id from ObjectFields where FieldName = 'RegionName')