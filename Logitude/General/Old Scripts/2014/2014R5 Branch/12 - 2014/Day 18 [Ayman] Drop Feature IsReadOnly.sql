
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'IsReadOnly')
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'IsReadOnly')
delete from Features where Code = 'IsReadOnly'
delete from TextCodes where Code = 'TenantManagement.Features.IsReadOnly'