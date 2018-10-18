delete from QueryColumns where QueryId=(Select Id from Queries where ObjectTableId=(Select Id from ObjectTables where Name ='BusinessRole'))
delete from Queries where ObjectTableId=(Select Id from ObjectTables where Name ='BusinessRole')
delete from PackageFeatures where FeatureId=(select Id  from Features where Code='BusinessRole.Q.BRAQ')
delete from RoleFeatures where FeatureId=(select Id from Features where Code='BusinessRole.Q.BRAQ')
delete from Features where Code='BusinessRole.Q.BRAQ'
