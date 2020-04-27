

update CustomerFieldsUpdateSettings set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = CustomerFieldsUpdateSettings.ObjectFieldId)