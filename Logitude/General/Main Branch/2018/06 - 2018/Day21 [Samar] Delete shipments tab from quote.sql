
delete from ObjectTableTabs where Code = 'QTSH'
delete from TextCodes where Code = 'Quote.TH.Shipments'

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'SHIPMENTS' and ObjectTableId = (select Id from ObjectTables where Name ='Quote'))
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'SHIPMENTS' and ObjectTableId = (select Id from ObjectTables where Name ='Quote'))
delete from Features where Code = 'SHIPMENTS' and ObjectTableId = (select Id from ObjectTables where Name ='Quote')