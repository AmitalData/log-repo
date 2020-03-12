

delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ExternalVATCard' and ObjectTableId = (select Id from ObjectTables where Name = 'VatType'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ExternalVATCard' and ObjectTableId = (select Id from ObjectTables where Name = 'VatType'))