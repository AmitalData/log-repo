
update ObjectFields set ShortNameTextCodeId = NULL where FieldName = 'House' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentAssembly')
update ObjectFields set ShortNameTextCodeId = NULL where FieldName = 'ShipperName' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentAssembly')

delete from TextCodes where Code = 'ShipmentAssembly.F.House.Short'
delete from TextCodes where Code = 'ShipmentAssembly.F.ShipperName.Short'


