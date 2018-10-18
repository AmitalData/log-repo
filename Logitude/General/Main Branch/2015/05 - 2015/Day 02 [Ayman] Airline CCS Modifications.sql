--After update

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Airline.CCSSettingsScreen' AND ObjectTableId = (select Id from ObjectTables where Name ='Airline'))
delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'Airline.CCSSettingsScreen' AND ObjectTableId = (select Id from ObjectTables where Name ='Airline'))
delete from Screens where Code = 'Airline.CCSSettingsScreen' AND ObjectTableId = (select Id from ObjectTables where Name ='Airline')
go

delete from TextCodes where Code = 'Airline.F.FSU' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.FSUHelpText' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.F.FHL' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.FHLHelpText' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.F.FWB' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.FWBHelpText' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.F.FSRFSA' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.FSRFSAHelpText' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.F.FVRFVA' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.FVRFVAHelpText' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.F.FFRFFA' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'Airline.FFRFFAHelpText' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
go

delete from ObjectFields where FieldName = 'FSU' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from ObjectFields where FieldName = 'FHL' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from ObjectFields where FieldName = 'FWB' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from ObjectFields where FieldName = 'FSRFSA' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from ObjectFields where FieldName = 'FVRFVA' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from ObjectFields where FieldName = 'FFRFFA' AND ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
go