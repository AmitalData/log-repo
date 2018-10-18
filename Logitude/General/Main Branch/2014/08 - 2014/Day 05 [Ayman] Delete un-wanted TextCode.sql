
-- Run this script then update Tenants
delete from RoleFeatures where FeatureId in (select Id from Features where Code = 'FOLLOWUPS')
go

delete from PackageFeatures where FeatureId in (select Id from Features where Code = 'FOLLOWUPS')
go

delete from QueryColumns where QueryId in (select Id from Queries where FeatureId in (select Id from Features where Code = 'FOLLOWUPS'))
go

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where FeatureId in (select Id from Features where Code = 'FOLLOWUPS'))
go

delete from Queries where FeatureId in (select Id from Features where Code = 'FOLLOWUPS')
go

delete from Features where Code = 'FOLLOWUPS'
go

delete from TextCodes where Code = 'General.Features.FollowUps'
go

delete from TextCodes where Code = 'Master.Features.FollowUps'
go

delete from RoleFeatures where FeatureId = (select id from dbo.Features where NameTextCodeId = (select id from TextCodes where Code = 'Quote.Features.Followups') )
go

delete from PackageFeatures where FeatureId = (select id from dbo.Features where NameTextCodeId = (select id from TextCodes where Code = 'Quote.Features.Followups') )
go

delete from Features where NameTextCodeId = (select id from TextCodes where Code = 'Quote.Features.Followups')
go

delete from TextCodes where Code = 'Quote.Features.Followups'
go