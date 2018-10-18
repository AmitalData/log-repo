delete from rolefeatures 
where featureid = (select id  from Features where code='ERRORLOGS')

delete from packagefeatures
where featureid = (select id  from Features where code='ERRORLOGS') 

delete from Features where code='ERRORLOGS'
delete from PackageFeatures where FeatureId= ( select id from  Features where code='ERRORLOG')



