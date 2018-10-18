update ObjectFields set DataTypeCode = 'nText' where FieldName = 'Notes' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentPickUpDelivery')
