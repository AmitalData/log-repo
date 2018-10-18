
delete from ObjectFields
where FieldName = 'CurrencyId'
and ObjectTableId in (select Id from ObjectTables where Name in ('ShipmentPayable', 'ShipmentReceivable'))

delete from Translations where TextCodeId in (select id from TextCodes where Code like 'ShipmentPayable%.Currency%')
delete from Translations where TextCodeId in (select id from TextCodes where Code like 'ShipmentReceivable%.Currency%')

delete from TextCodes where Code like 'ShipmentPayable%.Currency%'
delete from TextCodes where Code like 'ShipmentReceivable%.Currency%'





