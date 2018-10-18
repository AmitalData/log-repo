update ObjectFields
set MaxLength = 2
where FieldName = 'Code' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')