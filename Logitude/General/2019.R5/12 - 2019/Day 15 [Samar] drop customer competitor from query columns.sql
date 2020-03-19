

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CompetitorFields')