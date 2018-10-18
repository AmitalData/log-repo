

delete from PackageFeatures where FeatureId = (select id from features where code = 'General.Features.EmailAlertSetting')
delete from MenusTables where FeatureId = (select id from features where code = 'General.Features.EmailAlertSetting')
delete from RoleFeatures where FeatureId = (select id from features where code = 'General.Features.EmailAlertSetting')


delete from features where code = 'General.Features.EmailAlertSetting'
 