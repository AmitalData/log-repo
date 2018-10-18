
delete from ObjectFields where FieldName = 'Contacts' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from ObjectFields where FieldName = 'LoginNotes' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from ObjectFields where FieldName = 'ConnectedToTenantId' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')

delete from TextCodes where TextCodeTypeCode = 'F' and Code like '%Contacts%' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where TextCodeTypeCode = 'F' and Code like '%LoginNotes%' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where TextCodeTypeCode = 'F' and Code like '%ConnectedToTenantId%' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')

delete from TextCodes where TextCodeTypeCode = 'H' and Code like '%Contacts%' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where TextCodeTypeCode = 'H' and Code like '%LoginNotes%' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where TextCodeTypeCode = 'H' and Code like '%ConnectedToTenantId%' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')