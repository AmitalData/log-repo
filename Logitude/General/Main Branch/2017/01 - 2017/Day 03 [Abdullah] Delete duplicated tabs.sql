-- USAGE: delete tab and features
delete from RoleFeatures where FeatureId = (select Id from Features where Code='DETAILS' and ObjectTableId = (select Id from ObjectTables where Name='bankdeposit'))
delete from PackageFeatures where FeatureId = (select Id from Features where Code='DETAILS' and ObjectTableId = (select Id from ObjectTables where Name='bankdeposit'))
delete from Features where Code = 'DETAILS' and ObjectTableId = (select Id from ObjectTables where Name='bankdeposit')
delete from ObjectTableTabs where Code = 'BDDT' and ObjectTableId = (select Id from ObjectTables where Name='bankdeposit')

-- update accounting