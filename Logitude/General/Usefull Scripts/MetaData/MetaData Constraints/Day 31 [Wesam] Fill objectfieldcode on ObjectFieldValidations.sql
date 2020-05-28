

update ObjectFieldValidations set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ObjectFieldValidations.ObjectFieldId)