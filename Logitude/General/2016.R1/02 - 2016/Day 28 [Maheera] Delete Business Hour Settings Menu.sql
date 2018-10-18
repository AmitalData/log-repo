delete from PackageFeatures where FeatureId = (select id  from Features where FeatureTypeCode = 'MENU' and code = 'General.Features.BusinessHour')
delete from RoleFeatures where FeatureId = (select id  from Features where FeatureTypeCode = 'MENU' and code = 'General.Features.BusinessHour')
delete  from Features where FeatureTypeCode = 'MENU' and code = 'General.Features.BusinessHour'
