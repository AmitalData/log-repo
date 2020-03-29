
delete from ObjectFields where FieldName = 'Logo' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.F.Logo'
delete from TextCodes where Code = 'Airline.LogoHelpText'