
update ObjectFields
set CanFilter = 0
where FieldName = 'ClassifierId'

delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ClassifierId' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))