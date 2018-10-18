

delete  ObjectFields where FieldName = 'UpdateByUserId' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')