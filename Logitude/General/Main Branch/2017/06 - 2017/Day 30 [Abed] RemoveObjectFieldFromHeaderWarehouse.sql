declare @ObjectFieldId as varchar(15)
set @ObjectFieldId = (select Id from ObjectFields where FieldName = 'ShipmentNumber' and ObjectTableId = (select Id from ObjectTables where Name ='WarehouseRelease'))
delete  ScreenFields where ObjectFieldId = @ObjectFieldId
set @ObjectFieldId = (select Id from ObjectFields where FieldName = 'ShipmentNumber' and ObjectTableId = (select Id from ObjectTables where Name ='WarehouseEntry'))
delete  ScreenFields where ObjectFieldId = @ObjectFieldId