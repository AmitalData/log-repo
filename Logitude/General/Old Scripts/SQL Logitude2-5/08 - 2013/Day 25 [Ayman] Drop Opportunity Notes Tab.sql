

delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'Opportunity.Tab.Notes' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity'))
go

delete from ObjectTableTabs where Code = 'OPNO' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete Features where Code = 'Opportunity.Tab.Notes' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code = 'Opportunity.Features.Notes'
go

delete from TextCodes where Code = 'Opportunity.TH.Notes'
go




