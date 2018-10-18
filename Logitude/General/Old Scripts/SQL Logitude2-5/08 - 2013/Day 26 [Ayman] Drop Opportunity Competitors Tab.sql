
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'Opportunity.Tab.Competitors' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity'))
go

delete from ObjectTableTabs where Code = 'OPCO' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete Features where Code = 'Opportunity.Tab.Competitors' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code = 'Opportunity.Features.Competitors'
go

delete from TextCodes where Code = 'Opportunity.TH.Competitors'
go