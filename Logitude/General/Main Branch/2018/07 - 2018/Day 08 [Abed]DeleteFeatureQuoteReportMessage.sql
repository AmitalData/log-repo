
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'QUOTETEMPLATEMESSAGE' and ObjectTableId = (Select Id from ObjectTables where Name = 'ReportsTemplate'))
go

delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'QUOTETEMPLATEMESSAGE' and ObjectTableId = (Select Id from ObjectTables where Name = 'ReportsTemplate'))
go

delete Features where Code = 'QUOTETEMPLATEMESSAGE' and ObjectTableId = (Select Id from ObjectTables where Name = 'ReportsTemplate')
go

delete from TextCodes where Code = 'ReportsTemplate.Features.QuoteTemplateMessage'
go
