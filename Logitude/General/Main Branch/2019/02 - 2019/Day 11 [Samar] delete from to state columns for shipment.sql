
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'MainCarriageFromState')
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'MainCarriageToState')