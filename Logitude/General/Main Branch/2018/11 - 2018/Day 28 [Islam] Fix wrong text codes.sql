--select * from objecttables
--select * from objectfields where fieldname = 'IncotermId'and objecttableid in (select id from objecttables where name = 'shipment')
--select * from textcodes where id = '1-201' or id = '1-200'
update textcodes set code = 'Shipment.IncotermHelpText' where code=  'Shipment.IncotermIdHelpText'

--select * from objectfields where fieldname = 'UpdatedByUserId'and objecttableid in (select id from objecttables where name = 'ShipmentAssembly')


--select * from textcodes where objecttableid in (select id from objecttables where name = 'ShipmentAssembly')


update textcodes set code = 'ShipmentAssembly.F.CreatedByUserId' where code = 'Shipment.F.CreatedByUserId' and objecttableid in (select id from objecttables where name = 'ShipmentAssembly')
update textcodes set code = 'ShipmentAssembly.CreatedByUserIdHelpText' where code = 'Shipment.CreatedByUserIdHelpText' and objecttableid in (select id from objecttables where name = 'ShipmentAssembly')

update textcodes set code = 'ShipmentAssembly.F.UpdatedByUserId' where code = 'Shipment.F.UpdatedByUserId' and objecttableid in (select id from objecttables where name = 'ShipmentAssembly')
update textcodes set code = 'ShipmentAssembly.UpdatedByUserIdHelpText' where code = 'Shipment.UpdatedByUserIdHelpText' and objecttableid in (select id from objecttables where name = 'ShipmentAssembly')


--select * from textcodes where code =  'ShipmentAssembly.CreatedByUserIdHelpText'
 

update textcodes set code = 'ComputingPartner.F.InActive' where code = 'BusinessUnit.F.InActive' and objecttableid in (select id from objecttables where name = 'ComputingPartner')
update textcodes set code = 'ComputingPartner.InActiveHelpText' where code = 'BusinessUnit.InActiveHelpText' and objecttableid in (select id from objecttables where name = 'ComputingPartner')
update textcodes set code = 'ComputingPartner.CH.InActiveListLable' where code = 'BusinessUnit.CH.InActiveListLable' and objecttableid in (select id from objecttables where name = 'ComputingPartner')

update textcodes set code = 'ShipmentType.F.TransportModeId'where code = 'TransportModeId.F.TransportModeId' and objecttableid in (select id from objecttables where name = 'ShipmentType')
update textcodes set code = 'ShipmentType.TransportModeIdHelpText'where code = 'TransportModeId.TransportModeIdHelpText' and objecttableid in (select id from objecttables where name = 'ShipmentType')


update textcodes set code = 'SystemData.F.Supportemail' where code = 'SystemData.F.Support e-mail' and objecttableid in (select id from objecttables where name = 'SystemData')
update textcodes set code = 'SystemData.SupportemailHelpText' where code = 'SystemData.Support e-mailHelpText' and objecttableid in (select id from objecttables where name = 'SystemData')
update textcodes set code = 'Warehouse.F.SearchFields'where code = 'WareHouse.F.SearchFields' and objecttableid in (select id from objecttables where name = 'warehouse')
update textcodes set code = 'Warehouse.SearchFieldsHelpText'where code = 'WareHouse.SearchFieldsHelpText' and objecttableid in (select id from objecttables where name = 'warehouse')

update textcodes set code = 'RatesTable.F.SearchFields'where code = 'RatesTableObject.Name.F.SearchFields' and objecttableid in (select id from objecttables where name = 'ratestable')
update textcodes set code = 'RatesTable.SearchFieldsHelpText'where code = 'RatesTableObject.Name.SearchFieldsHelpText' and objecttableid in (select id from objecttables where name = 'ratestable')

 

update   ObjectFields set IsCustom =0,IsCustomFilter=1 where FieldName = 'ContactIdCustomFilter'



--select * from Screens where Code like 'TenantManagement.%'and  ObjectTableId in (select id from ObjectTables where Name = 'tenant')
delete from ScreenFields where ScreenId in (select id from Screens where Code like 'TenantManagement.%'and  ObjectTableId in (select id from ObjectTables where Name = 'tenant'))
delete from Screens where id in (select id from Screens where Code like 'TenantManagement.%'and  ObjectTableId in (select id from ObjectTables where Name = 'tenant'))




delete from MenuButtons where LabelTextCodeId in (select id from TextCodes where Code = 'Quote.B.QuoteCopySeparator' and ObjectTableId not in (select id from objecttables where name = 'quote'))
delete from TextCodes where Code = 'Quote.B.QuoteCopySeparator' and ObjectTableId not in (select id from objecttables where name = 'quote')

delete from MenuButtons where LabelTextCodeId in (select id from TextCodes where Code = 'Quote.MenuButtons.QuoteBuildShipmentSeparator' and ObjectTableId not in (select id from objecttables where name = 'quote'))
delete from TextCodes where Code = 'Quote.MenuButtons.QuoteBuildShipmentSeparator' and ObjectTableId not in (select id from objecttables where name = 'quote')


update textcodes set code = 'CustomerTenantAccess.F.ContactName' where code = 'CustomerTenantAccess.F.Contact' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.ContactHelpText' where code = 'CustomerTenantAccess.ContactNameHelpText' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.CompanyVat' where code = 'CustomerTenantAccess.F.Company VAT' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')

update textcodes set code = 'CustomerTenantAccess.F.CompanyName' where code = 'CustomerTenantAccess.F.Company' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.CompanyEmail' where code = 'CustomerTenantAccess.F.Company Email' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.ContactMobile' where code = 'CustomerTenantAccess.F.Contact Mobile' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.ContactPhone' where code = 'CustomerTenantAccess.F.Contact Phone' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.RequestDateTime' where code = 'CustomerTenantAccess.F.Request Date' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')

update textcodes set code = 'CustomerTenantAccess.F.StockTypeCode' where code = 'CustomerTenantAccess.F.Stock Type' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.UpdatedByUserId' where code = 'CustomerTenantAccess.F.Updated By User' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.UpdatedByUserName' where code = 'CustomerTenantAccess.F.UpdatedByUser' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')

update textcodes set code = 'CustomerTenantAccess.F.LastUpdateDate' where code = 'CustomerTenantAccess.F.Last Update Date' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')
update textcodes set code = 'CustomerTenantAccess.F.LastShipmentDate' where code = 'CustomerTenantAccess.F.Last Shipment Date' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')

update textcodes set code = 'CustomerTenantAccess.CH.CompanyVatListLable' where code = 'CustomerTenantAccess.CH.CompanyVATListLable' and objecttableid in (select id from objecttables where name = 'CustomerTenantAccess')

