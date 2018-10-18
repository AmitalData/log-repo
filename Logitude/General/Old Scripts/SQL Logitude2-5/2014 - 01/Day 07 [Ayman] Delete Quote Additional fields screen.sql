

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'NewQuote' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'NewQuote' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from Screens where Code = 'NewQuote' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go