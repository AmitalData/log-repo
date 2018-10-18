

delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'Opportunity.Tab.Communications' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity'))
go

delete Features where Code = 'Opportunity.Tab.Communications' and ObjectTableId = (Select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code = 'Opportunity.Features.Communications'
go


delete from objecttabletabs where code = 'OPCM'
go

delete from TextCodes where Code = 'Opportunity.TH.Communications'
go



