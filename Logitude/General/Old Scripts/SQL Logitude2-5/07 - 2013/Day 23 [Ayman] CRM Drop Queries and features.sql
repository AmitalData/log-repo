
-- Activity
delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (Select Id from ObjectTables where Name = 'Activity'))
go

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (Select Id from ObjectTables where Name = 'Activity'))
go

delete from Queries where ObjectTableId = (Select Id from ObjectTables where Name = 'Activity')
go

delete from RoleFeatures where FeatureId in (Select Id from Features where ObjectTableId = (Select Id from ObjectTables where Name = 'Activity'))
go

delete from Features where FeatureTypeCode = 'QUER' and ObjectTableId = (Select Id from ObjectTables where Name = 'Activity')
go

delete from TextCodes where Code = 'Activity.Features.AllTasks'
go

delete from TextCodes where Code  = 'Activity.Features.MyActivities'
go

delete from TextCodes where Code  = 'Activity.Features.AllActivities'
go

delete from TextCodes where Code  = 'Activity.Q.AllTasks'
go

delete from TextCodes where Code  = 'Activity.Q.MyActivities'
go

delete from TextCodes where Code  = 'Activity.Q.AllActivities'
go



-- Opportunity
delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity'))
go

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity'))
go

delete from Queries where ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete from RoleFeatures where FeatureId in (Select Id from Features where ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity'))
go

delete from Features where FeatureTypeCode = 'QUER' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code = 'Opportunity.Features.MyOpportunities'
go

delete from TextCodes where Code = 'Opportunity.Features.AllOpportunities'
go

delete from TextCodes where Code  = 'Opportunity.Q.MyOpportunities'
go

delete from TextCodes where Code  = 'Opportunity.Q.AllOpportunities'
go