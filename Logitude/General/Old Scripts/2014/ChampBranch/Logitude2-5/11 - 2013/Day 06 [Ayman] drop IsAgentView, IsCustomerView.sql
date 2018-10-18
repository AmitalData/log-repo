
delete from ScreenFields where ObjectFieldId  = (select Id from ObjectFields where FieldName = 'IsAgentView' and ObjectTableId = (select Id from ObjectTables where Name = 'DocumentType'))

delete from ScreenFields where ObjectFieldId  = (select Id from ObjectFields where FieldName = 'IsCustomerView' and ObjectTableId = (select Id from ObjectTables where Name = 'DocumentType'))

