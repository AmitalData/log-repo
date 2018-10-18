delete from querycolumns where ObjectFieldId = ( select id from ObjectFields where FieldName ='DeclarationStatusCode')
delete from ScreenFields where ObjectFieldId = ( select id from ObjectFields where FieldName ='DeclarationStatusCode')
delete from ObjectFields where FieldName = 'DeclarationStatusCode'