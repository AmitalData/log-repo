
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'Shipment.ConnectedTo')
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'Shipment.ConnectedTo')
delete from Features where Code = 'Shipment.ConnectedTo'