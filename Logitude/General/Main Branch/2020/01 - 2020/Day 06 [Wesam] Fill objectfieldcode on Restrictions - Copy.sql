
update Restrictions set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = Restrictions.ObjectFieldId)
