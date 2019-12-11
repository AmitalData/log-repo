

update ScreenFields set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ScreenFields.ObjectFieldId)