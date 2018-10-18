delete from RoleFeatures where FeatureId = (select Id from Features where Code='GENERAL' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount'))
delete from RoleFeatures where FeatureId = (select Id from Features where Code='EVENTS' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount'))

delete from PackageFeatures where FeatureId = (select Id from Features where Code='GENERAL' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount'))
delete from PackageFeatures where FeatureId = (select Id from Features where Code='EVENTS' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount'))

delete from Features where Code='GENERAL' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount')
delete from Features where Code='EVENTS' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount')

delete from ObjectTableTabs where Code = 'CAGC' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount')
delete from ObjectTableTabs where Code = 'CAEV' and ObjectTableId = (select Id from ObjectTables where Name='ChartOfAccount')

