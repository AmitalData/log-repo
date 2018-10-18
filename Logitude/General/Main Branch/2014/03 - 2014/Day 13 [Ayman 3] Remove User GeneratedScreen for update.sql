
-- Run this SQL, Then Update Tenants

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'User.GeneralTabScreen' AND ObjectTableId = (select Id from ObjectTables where Name = 'User'))
go

delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'User.GeneralTabScreen' AND ObjectTableId = (select Id from ObjectTables where Name = 'User'))
go

delete from Screens where Code = 'User.GeneralTabScreen' AND ObjectTableId = (select Id from ObjectTables where Name = 'User')
go