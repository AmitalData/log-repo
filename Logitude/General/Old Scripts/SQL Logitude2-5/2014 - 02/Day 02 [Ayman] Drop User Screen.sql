

-- Run this script and update all tenants

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'User.GeneralTabScreen' and ObjectTableId = (select Id from ObjectTables where Name = 'User'))
go

delete from Screens where Code = 'User.GeneralTabScreen' and ObjectTableId = (select Id from ObjectTables where Name = 'User')
go