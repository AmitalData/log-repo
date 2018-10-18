



delete PackageFeatures where FeatureId = (select id from Features where NameTextCodeId in (select id from TextCodes where DefaultText = 'Connected Shipments' and ObjectTableId = (select id from ObjectTables where Name = 'WarehouseEntry')))
delete RoleFeatures where FeatureId = (select id from Features where NameTextCodeId in (select id from TextCodes where DefaultText = 'Connected Shipments' and ObjectTableId = (select id from ObjectTables where Name = 'WarehouseEntry')))
delete Features where NameTextCodeId in (select id from TextCodes where DefaultText = 'Connected Shipments' and ObjectTableId = (select id from ObjectTables where Name = 'WarehouseEntry'))
delete TextCodes where DefaultText = 'Connected Shipments' and ObjectTableId = (select id from ObjectTables where Name = 'WarehouseEntry')
