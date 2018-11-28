
--USAGE: delete tab and features
delete from RoleFeatures where FeatureId = (select Id from Features where Code='GENERAL' and ObjectTableId = (select Id from ObjectTables where Name='journal'))
delete from PackageFeatures where FeatureId = (select Id from Features where Code='GENERAL' and ObjectTableId = (select Id from ObjectTables where Name='journal'))
delete from Features where Code = 'GENERAL' and ObjectTableId = (select Id from ObjectTables where Name='journal')
delete from ObjectTableTabs where Code = 'JNGC' and ObjectTableId = (select Id from ObjectTables where Name='journal')