
delete from QueryColumns where QueryId = ( select Id from Queries where Code = 'Follow Ups' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from AdvancedQueryFilters where QueryId = ( select Id from Queries where Code = 'Follow Ups' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from Queries where Code = 'Follow Ups' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'FOLLOWUPS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'FOLLOWUPS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from Features where Code = 'FOLLOWUPS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')