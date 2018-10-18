
delete from ObjectFields where FieldName = 'OrganizerId' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')

delete from TextCodes where Code like '%Organizer%' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')