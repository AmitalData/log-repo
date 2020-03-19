
delete from RoleFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineAreasPort'))
delete from PackageFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineAreasPort'))
delete from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineAreasPort')
delete from EventTypes where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineAreasPort')
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineAreasPort')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineAreasPort')
delete from ObjectTables where Name = 'AirlineAreasPort'


delete from RoleFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineArea'))
delete from PackageFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineArea'))
delete from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineArea')
delete from EventTypes where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineArea')
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineArea')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'AirlineArea')
delete from ObjectTables where Name = 'AirlineArea'