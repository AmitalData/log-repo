
delete from ObjectTableTabs where Code = 'CLVW' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')

delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'OVERVIEW_2' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'OVERVIEW_2' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
delete from Features where Code = 'OVERVIEW_2' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')

delete from TextCodes where Code = 'Customer.TH.Overview_2'

