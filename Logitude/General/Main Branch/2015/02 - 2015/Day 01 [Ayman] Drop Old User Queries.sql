

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'User'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'User'))
go

delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'User') AND (OriginalQueryId is not null)
go

delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'User')
go

delete from RoleFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'User') AND (Code = 'Users' OR Code = 'LICENSEDUSERS'))
go

delete from PackageFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'User') AND (Code = 'Users' OR Code = 'LICENSEDUSERS'))
go

delete from Features where ObjectTableId = (select Id from ObjectTables where Name = 'User') AND (Code = 'Users' OR Code = 'LICENSEDUSERS')
go

delete from TextCodes where Code = 'User.Q.Users'
delete from TextCodes where Code = 'User.Q.LicensedUsers'
delete from TextCodes where Code = 'User.Features.Users'
delete from TextCodes where Code = 'User.Features.LicensedUsers'