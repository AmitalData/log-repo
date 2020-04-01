

update QueryColumns set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = QueryColumns.ObjectFieldId)