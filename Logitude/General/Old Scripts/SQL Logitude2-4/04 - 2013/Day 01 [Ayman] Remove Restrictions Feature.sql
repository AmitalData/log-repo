

delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'RESTRICTIONS')
go

delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'RESTRICTIONS')
go

delete from Features where Code = 'RESTRICTIONS'
go

delete from TextCodes where Code = 'User.TH.Restrictions'
go

delete from TextCodes where Code = 'User.Features.Restrictions'
go