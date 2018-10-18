

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and FieldName = 'FromAddressId'
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and FieldName = 'ToAddressId'
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and FieldName = 'CustomerAddressId'
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and FieldName = 'ShipperAddressId'
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and FieldName = 'ConsigneeAddressId'
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and FieldName = 'IncludePickUp'
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and FieldName = 'IncludeDelivery'

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code like '%FromAddressId%'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code like '%ToAddressId%'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code like '%CustomerAddress%'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code like '%ShipperAddress%'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code like '%ConsigneeAddress%'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code = 'Quote.F.IncludePickUp'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code = 'Quote.IncludePickUpHelpText'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code = 'Quote.F.IncludeDelivery'
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Quote') and Code = 'Quote.IncludeDeliveryHelpText'