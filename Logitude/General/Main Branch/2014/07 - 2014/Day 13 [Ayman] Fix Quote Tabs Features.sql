

-- Run this Script then update Tenants
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'ORDERS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'ORDERS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from ObjectTableTabs where Code = 'QTOR'
go

delete from Features where Code = 'ORDERS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

delete from TextCodes where Code = 'Quote.Features.Orders'
go

delete from TextCodes where Code = 'Quote.TH.Orders'
go