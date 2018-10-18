
update ObjectFields set MaxLength = 250 where ObjectTableId = (select Id from ObjectTables where Name = 'Customer') and FieldName = 'RequestedAirlines'
update ObjectFields set MaxLength = 250 where ObjectTableId = (select Id from ObjectTables where Name = 'Customer') and FieldName = 'RegisteredAirlines'
update ObjectFields set MaxLength = 250 where ObjectTableId = (select Id from ObjectTables where Name = 'Customer') and FieldName = 'PendingAirlines'
