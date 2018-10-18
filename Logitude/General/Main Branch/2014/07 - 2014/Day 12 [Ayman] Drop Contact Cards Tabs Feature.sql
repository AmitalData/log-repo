
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'CARDS')
go

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'CARDS')
go

delete from Features where Code = 'CARDS'
go

delete from TextCodes where Code = 'Contact.Features.Cards'
go

delete from ObjectTableTabs where Code = 'COCA'
go

