--shipment
delete from ObjectTableRuleFields where ObjectFieldId = ( select Id from objectfields where fieldname = 'AWBPrintSpecificationCode' and ObjectTableId = (select Id from ObjectTables where Name = 'shipment'))
delete from ObjectFields where FieldName = 'AWBPrintSpecificationCode' and ObjectTableId = (select Id from ObjectTables where Name = 'shipment')
delete from TextCodes where Code = 'shipment.f.AWBPrintSpecificationCode'

--master
delete from ObjectTableRuleFields where ObjectFieldId = ( select Id from objectfields where fieldname = 'AWBPrintSpecificationCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Master'))
delete from ObjectFields where FieldName = 'AWBPrintSpecificationCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Master')
delete from TextCodes where Code = 'Master.f.AWBPrintSpecificationCode'

--objectFields, objectTable and textCodes
delete from ObjectFields where FieldName = 'Code' and ObjectTableId = (select Id from ObjectTables where Name = 'AWBPrintSpecification')
delete from ObjectFields where FieldName = 'Name' and ObjectTableId = (select Id from ObjectTables where Name = 'AWBPrintSpecification')
delete from ObjectFields where FieldName = 'SearchFields' and ObjectTableId = (select Id from ObjectTables where Name = 'AWBPrintSpecification')

delete from TextCodes where Code = 'AWBPrintSpecification.F.SearchFields'
delete from TextCodes where Code = 'AWBPrintSpecification.SearchFieldsHelpText'
delete from TextCodes where Code = 'AWBPrintSpecification.CodeHelpText'
delete from TextCodes where Code = 'AWBPrintSpecification.CH.CodeListLable'
delete from TextCodes where Code = 'AWBPrintSpecification.F.Name'
delete from TextCodes where Code = 'AWBPrintSpecification.NameHelpText'
delete from TextCodes where Code = 'AWBPrintSpecification.CH.NameListLable'
delete from TextCodes where Code = 'AWBPrintSpecification.F.Code'

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'AWBPrintSpecification')
delete from ObjectTables where Name = 'AWBPrintSpecification'

--database
alter table shipments drop constraint [FK_ShipmentAWBPrintSpecification]
go

drop index [IX_FK_ShipmentAWBPrintSpecification] on shipments
go

alter table shipments drop column AWBPrintSpecificationCode
go

drop table AWBPrintSpecifications

