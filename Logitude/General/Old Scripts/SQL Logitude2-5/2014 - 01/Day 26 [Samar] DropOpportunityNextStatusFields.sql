
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'NextActivityStatusName')
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'NextActivityStatusCode')

delete from ObjectFields where FieldName = 'NextActivityStatusName'
delete from ObjectFields where FieldName = 'NextActivityStatusCode'

delete from TextCodes where Code like '%NextActivityStatusName%'
delete from TextCodes where Code like '%NextActivityStatusCode%'