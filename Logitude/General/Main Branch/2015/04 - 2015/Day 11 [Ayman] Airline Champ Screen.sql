-- Run after update 

delete from Rolefeatures where FeatureId = (select Id from features where Code = 'CHAMP' AND ObjectTableId = (select Id from ObjectTables where name = 'airline'))
go

delete from PackageFeatures where FeatureId = (select Id from features where Code = 'CHAMP' AND ObjectTableId = (select Id from ObjectTables where name = 'airline'))
go

delete from features where Code = 'CHAMP' AND ObjectTableId = (select Id from ObjectTables where name = 'airline')
go

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Airline.ChampTabScreen')
go

delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'Airline.ChampTabScreen')
go

delete from Screens where Code = 'Airline.ChampTabScreen'
go

delete from TextCodes where Code = 'Airline.TH.Champ'
go

delete from TextCodes where Code = 'Airline.Features.Champ'
go