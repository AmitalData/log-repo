
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity'))
go

delete Features where Code = 'General' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code = 'Opportunity.Features.General'
go
