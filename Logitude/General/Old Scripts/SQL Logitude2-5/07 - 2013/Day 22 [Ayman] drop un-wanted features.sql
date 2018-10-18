

delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'Activity'))
go

delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'LeadSource'))
go

delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'Stage'))
go

delete Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'Activity')
go

delete Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'LeadSource')
go

delete Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'Stage')
go

delete from TextCodes where Code = 'Activity.Features.General'
go

delete from TextCodes where Code = 'LeadSource.Features.General'
go

delete from TextCodes where Code = 'Stage.Features.General'
go