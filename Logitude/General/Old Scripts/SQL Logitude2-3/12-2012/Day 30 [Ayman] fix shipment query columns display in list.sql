
-- Update will fix it 

update ObjectFields set DisplayInList = 0 where FieldName = 'AsAgreed' AND ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')
update ObjectFields set DisplayInList = 0 where FieldName = 'EntityStatusName' AND ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')