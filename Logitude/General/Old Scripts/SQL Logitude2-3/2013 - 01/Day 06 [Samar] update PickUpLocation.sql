update ObjectFields 
set MaxLength = 500
where FieldName = 'PickupLocation' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

update ObjectFields 
set MaxLength = 250
where FieldName = 'PickUpAddress' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

update ObjectFields 
set MaxLength = 250
where FieldName = 'DeliveryAddress' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')