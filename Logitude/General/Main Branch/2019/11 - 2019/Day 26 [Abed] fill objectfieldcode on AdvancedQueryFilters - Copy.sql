

update AdvancedQueryFilters set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = AdvancedQueryFilters.ObjectFieldId)