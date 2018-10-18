delete from RoleFeatures where FeatureId=(select Id from Features where ObjectTableId=(select Id from ObjectTables where Name='ticket') and Code='Activate')
delete from PackageFeatures where FeatureId=(select Id from Features where ObjectTableId=(select Id from ObjectTables where Name='ticket') and Code='Activate')
delete from Features where ObjectTableId=(select Id from ObjectTables where Name='ticket') and Code='Activate'
