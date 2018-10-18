select * from ObjectFields where FieldName = 'AWBChargeAmount' and ObjectTableId = (select id from objecttables where name = 'shipment')
select * from ObjectFields where FieldName = 'AWBCommodityItemNumber' and ObjectTableId = (select id from objecttables where name = 'shipment')
select * from ObjectFields where FieldName = 'teu' and ObjectTableId = (select id from objecttables where name = 'shipment')


select * from ObjectFields where FieldName = 'AWBChargeAmount' and ObjectTableId = (select id from objecttables where name = 'shipment')
select * from ObjectFields where FieldName = 'AWBCommodityItemNumber' and ObjectTableId = (select id from objecttables where name = 'shipment')

delete from advancedqueryfilters where objectfieldid = (select id from ObjectFields where FieldName = 'AWBChargeAmount' and ObjectTableId = (select id from objecttables where name = 'shipment'))
delete from advancedqueryfilters where objectfieldid = (select id from ObjectFields where FieldName = 'AWBCommodityItemNumber' and ObjectTableId = (select id from objecttables where name = 'shipment'))



update ObjectFields set CanFilter = 0,DisplayInList = 0 where FieldName = 'teu' and ObjectTableId = (select id from objecttables where name = 'shipment')
update ObjectFields set CanFilter = 0,DisplayInList = 0 where FieldName = 'teu' and ObjectTableId = (select id from objecttables where name = 'shipment')


update ObjectFields set DataTypeCode = 'Double' where FieldName = 'teu' and ObjectTableId = (select id from objecttables where name = 'shipment')
select * from advancedqueryfilters where objectfieldid = (select id from ObjectFields where FieldName = 'teu' and ObjectTableId = (select id from objecttables where name = 'shipment'))
