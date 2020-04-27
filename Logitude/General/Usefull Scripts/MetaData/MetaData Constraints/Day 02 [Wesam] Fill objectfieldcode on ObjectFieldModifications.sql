

update ObjectFieldModifications set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ObjectFieldModifications.ObjectFieldId)