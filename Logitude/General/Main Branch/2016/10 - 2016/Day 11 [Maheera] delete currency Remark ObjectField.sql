delete from AdvancedQueryFilters where ObjectFieldId = (select id from ObjectFields where FieldName = 'Remark' and ObjectTableId = (select Id from ObjectTables where Name = 'Currency')) 

delete from ObjectFields where FieldName = 'Remark' and ObjectTableId = (select Id from ObjectTables where Name = 'Currency')

select * from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Currency')
