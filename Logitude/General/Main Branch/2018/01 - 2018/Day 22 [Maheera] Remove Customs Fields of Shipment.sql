delete from ObjectFields where ObjectTableId = (select id from objecttables where Name = 'Shipment') 
and
( 
FieldName = 'CustomsTransmissionsStatusCode'
or 
FieldName = 'CustomsTransmissionsStatusName'
or
FieldName = 'CustomsTransmissionsStatusDate'
or
FieldName = 'CustomsTransmissionsStatusError'
)

delete from TextCodes  where ObjectTableId = (select id from objecttables where Name = 'Shipment') and code like '%.CustomsTransmissions%'
