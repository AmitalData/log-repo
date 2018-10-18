
delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'RequesterId') 

delete from ObjectFields where FieldName = 'RequesterId'
delete from ObjectFields where FieldName = 'RequesterName'
delete from ObjectFields where FieldName = 'PotentialRequesterId'

delete from TextCodes where Code like '%Requester%'