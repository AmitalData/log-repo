update textcodes set code = 'ShipmentPayable.F.ChargesTypeId' where code = 'ShipmentPayable.f.chargestype' and objecttableid in (select id from objecttables where name = 'ShipmentPayable')
update textcodes set code = 'ShipmentReceivable.F.ChargesTypeId' where code = 'ShipmentReceivable.f.chargestype' and objecttableid in (select id from objecttables where name = 'ShipmentReceivable')


update textcodes set code = 'ShipmentPayable.F.MeasurementId' where code = 'ShipmentPayable.f.Measurement' and objecttableid in (select id from objecttables where name = 'ShipmentPayable')
update textcodes set code = 'ShipmentReceivable.F.MeasurementId' where code = 'ShipmentReceivable.f.Measurement' and objecttableid in (select id from objecttables where name = 'ShipmentReceivable')

update textcodes set code = 'Card.F.EnglishName' where code = 'Card.f.English Name' and objecttableid in (select id from objecttables where name = 'Card')

 update objecttables set cacheonclient = 0 where name in ('ARInvoicePayment','SATInterfaceSetting','TenantManagmentPrivateLabels','DocumentFilingBackupSetting','BankAccountLite')


 delete from textcodes where code = 'arinvoice.issuedbyuseridhelptext' and objecttableid in (select id from objecttables where name = 'arinvoice')
delete from textcodes where code = 'arinvoice.issuedbyusernamehelptext' and objecttableid in (select id from objecttables where name = 'arinvoice')
--delete from textcodes where code = 'ARInvoice.CH.IssuedByUserNameListLable' and objecttableid in (select id from objecttables where name = 'arinvoice')

delete from textcodes where code = 'shipment.payablesinlocalcurrencyhelptext' and objecttableid in (select id from objecttables where name = 'shipment')
delete from textcodes where code = 'shipment.CH.payablesinlocalcurrencyListLable' and objecttableid in (select id from objecttables where name = 'shipment')
delete from textcodes where code = 'currency.remarkhelptext' and objecttableid in (select id from objecttables where name = 'currency')

