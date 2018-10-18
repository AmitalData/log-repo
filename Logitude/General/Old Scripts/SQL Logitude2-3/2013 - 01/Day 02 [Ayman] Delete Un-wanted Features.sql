
delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'OPENHOUSEANDDIRECT')
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'OPENHOUSEANDDIRECT')
delete from Features where Code = 'OPENHOUSEANDDIRECT'

delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'OPENMASTERANDDIRECT')
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'OPENMASTERANDDIRECT')
delete from Features where Code = 'OPENMASTERANDDIRECT'

delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'ACCOUNTINGHOUSEANDDIRECT')
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'ACCOUNTINGHOUSEANDDIRECT')
delete from Features where Code = 'ACCOUNTINGHOUSEANDDIRECT'

delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'ACCOUNTINGMASTERANDDIRECT')
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'ACCOUNTINGMASTERANDDIRECT')
delete from Features where Code = 'ACCOUNTINGMASTERANDDIRECT'