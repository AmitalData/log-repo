delete from  features where code='ERRORLOGS'
delete from PackageFeatures where FeatureId= ( select id from Features where code='ERRORLOGS')
delete from  features where code='ERRORLOG'
delete from PackageFeatures where FeatureId= ( select id from Features where code='ERRORLOG')
