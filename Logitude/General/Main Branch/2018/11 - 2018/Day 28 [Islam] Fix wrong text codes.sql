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

update ObjectFields set FieldName = 'JournalNumber' where FieldName = 'Journal Number' and ObjectTableId in (select Id from ObjectTables where Name = 'apinvoice')

delete from Translations where TextCodeId in (select Id from TextCodes where Code = 'ARInvoice.CH.IssuedByUserNameListLable')
delete from TextCodes where Code = 'ARInvoice.CH.IssuedByUserNameListLable'

delete from TextCodes where Code = 'ARPayment.PaymentDateHelpText'

delete from Translations where TextCodeId in (select Id from TextCodes where Code = 'Quote.F.ShipperContactId.Short')
delete from TextCodes where Code = 'Quote.F.ShipperContactId.Short'

delete from TextCodes where Code = 'Quote.OrderNumberOfPackagesHelpText'

delete from Translations where TextCodeId in (select Id from TextCodes where Code = 'Quote.CH.FollowUpDateListLable')
delete from TextCodes where Code = 'Quote.CH.FollowUpDateListLable'

delete from Translations where TextCodeId in (select Id from TextCodes where Code = 'Quote.CH.FollowUpTypeListLable')
delete from TextCodes where Code = 'Quote.CH.FollowUpTypeListLable'

delete from Translations where TextCodeId in (select Id from TextCodes where Code = 'Quote.CH.FollowUpNotesListLable')
delete from TextCodes where Code = 'Quote.CH.FollowUpNotesListLable'

update TextCodes set Code = 'Shipment.Features.DOCSINDOWNLOADDOCUMENTS' where Code = 'Shipment.Features.DOCSINDownloadDocuments'

update TextCodes set Code = 'Address.F.CountryId' where Code = 'Address.F.Country'
update TextCodes set Code = 'Address.F.StateId' where Code = 'Address.F.State'
update TextCodes set Code = 'Address.F.PhoneNumber' where Code = 'Address.F.Phone'
update TextCodes set Code = 'Address.F.FaxNumber' where Code = 'Address.F.Fax'
update TextCodes set Code = 'User.F.CreateDate' where Code = 'User.F.Create Date'
delete  TextCodes where Code = 'Agent.AccountingCardHelpText'
delete  TextCodes where Code = 'Agent.CH.AccountingCardListLable'
delete  TextCodes where Code = 'Airline.AccountingCardHelpText'
delete  TextCodes where Code = 'Airline.CH.AccountingCardListLable'
delete  TextCodes where Code = 'Card.AccountingCardHelpText'
delete  TextCodes where Code = 'CustomAgent.AccountingCardHelpText'
delete  TextCodes where Code = 'CustomAgent.CH.AccountingCardListLable'
delete  TextCodes where Code = 'Customer.BillToNameHelpText'
delete  TextCodes where Code = 'Customer.AccountingCardHelpText'
delete from Translations where TextCodeId in (select Id from TextCodes where Code = 'Customer.CH.AccountingCardListLable')
delete  TextCodes where Code = 'Customer.CH.AccountingCardListLable'
delete  TextCodes where Code = 'ShippingAgent.AccountingCardHelpText'
delete  TextCodes where Code = 'ShippingAgent.CH.AccountingCardListLable'
delete  TextCodes where Code = 'ShippingLine.AccountingCardHelpText'
delete  TextCodes where Code = 'ShippingLine.CH.AccountingCardListLable'
delete  TextCodes where Code = 'Trucker.AccountingCardHelpText'
delete  TextCodes where Code = 'Trucker.CH.AccountingCardListLable'
delete  TextCodes where Code = 'Vendor.AccountingCardHelpText'
delete  TextCodes where Code = 'Vendor.CH.AccountingCardListLable'
delete  TextCodes where Code = 'Warehouse.AccountingCardHelpText'
delete  TextCodes where Code = 'Warehouse.CH.AccountingCardListLable'

update TextCodes set Code = 'User.CreateDateHelpText' where Code = 'User.Create DateHelpText'

update TextCodes set Code = 'MessagingStock.CH.TenantNumberLabel' where Code = 'AWBMessagingStock.CH.TenantNumberLabel'
update TextCodes set Code = 'MessagingStock.CH.TotalPriceLabel' where Code = 'AWBMessagingStock.CH.TotalPriceLabel'
update TextCodes set Code = 'MessagingStock.CH.UpdateDateLabel' where Code = 'AWBMessagingStock.CH.UpdateDateLabel'
update TextCodes set Code = 'MessagingStock.CreateDateHelpText' where Code = 'AWBMessagingStock.CreateDateHelpText'
update TextCodes set Code = 'MessagingStock.CreatedByUserIdHelpText' where Code = 'AWBMessagingStock.CreatedByUserIdHelpText'
update TextCodes set Code = 'MessagingStock.EndDateHelpText' where Code = 'AWBMessagingStock.EndDateHelpText'
update TextCodes set Code = 'MessagingStock.F.Amount' where Code = 'AWBMessagingStock.F.Amount'
update TextCodes set Code = 'MessagingStock.F.CreateDate' where Code = 'AWBMessagingStock.F.CreateDate'
update TextCodes set Code = 'MessagingStock.F.CreatedByUserId' where Code = 'AWBMessagingStock.F.CreatedByUserId'
update TextCodes set Code = 'MessagingStock.F.EndDate' where Code = 'AWBMessagingStock.F.EndDate'
update TextCodes set Code = 'MessagingStock.AmountHelpText' where Code = 'AWBMessagingStock.AmountHelpText'
update TextCodes set Code = 'MessagingStock.B.Cancel' where Code = 'AWBMessagingStock.B.Cancel'
update TextCodes set Code = 'MessagingStock.B.More' where Code = 'AWBMessagingStock.B.More'
update TextCodes set Code = 'MessagingStock.CH.AmountLabel' where Code = 'AWBMessagingStock.CH.AmountLabel'
update TextCodes set Code = 'MessagingStock.CH.CreateDateLabel' where Code = 'AWBMessagingStock.CH.CreateDateLabel'
update TextCodes set Code = 'MessagingStock.CH.EndDateLabel' where Code = 'AWBMessagingStock.CH.EndDateLabel'
update TextCodes set Code = 'MessagingStock.CH.NotesLabel' where Code = 'AWBMessagingStock.CH.NotesLabel'
update TextCodes set Code = 'MessagingStock.CH.StartDateLabel' where Code = 'AWBMessagingStock.CH.StartDateLabel'
update TextCodes set Code = 'MessagingStock.CH.StatusLabel' where Code = 'AWBMessagingStock.CH.StatusLabel'
update TextCodes set Code = 'MessagingStock.CH.TenantNameLabel' where Code = 'AWBMessagingStock.CH.TenantNameLabel'
update TextCodes set Code = 'MessagingStock.Features.AllAWBMessagingStocks' where Code = 'AWBMessagingStock.Features.AllAWBMessagingStocks'
update TextCodes set Code = 'MessagingStock.Features.AWBMessagingStocks' where Code = 'AWBMessagingStock.Features.AWBMessagingStocks'
update TextCodes set Code = 'MessagingStock.Features.Cancel' where Code = 'AWBMessagingStock.Features.Cancel'
update TextCodes set Code = 'MessagingStock.Features.Edit' where Code = 'AWBMessagingStock.Features.Edit'
update TextCodes set Code = 'MessagingStock.Features.Events' where Code = 'AWBMessagingStock.Features.Events'
update TextCodes set Code = 'MessagingStock.Features.General' where Code = 'AWBMessagingStock.Features.General'
update TextCodes set Code = 'MessagingStock.Features.New' where Code = 'AWBMessagingStock.Features.New'
update TextCodes set Code = 'MessagingStock.Features.PackageFeature' where Code = 'AWBMessagingStock.Features.PackageFeature'
update TextCodes set Code = 'MessagingStock.Features.Read' where Code = 'AWBMessagingStock.Features.Read'
update TextCodes set Code = 'MessagingStock.IsCancelledHelpText' where Code = 'AWBMessagingStock.IsCancelledHelpText'
update TextCodes set Code = 'MessagingStock.TH.General' where Code = 'AWBMessagingStock.TH.General'
update TextCodes set Code = 'MessagingStock.TH.Events' where Code = 'AWBMessagingStock.TH.Events'
update TextCodes set Code = 'MessagingStock.TenantNumberHelpText' where Code = 'AWBMessagingStock.TenantNumberHelpText'
update TextCodes set Code = 'MessagingStock.TenantNameHelpText' where Code = 'AWBMessagingStock.TenantNameHelpText'
update TextCodes set Code = 'MessagingStock.StatusHelpText' where Code = 'AWBMessagingStock.StatusHelpText'
update TextCodes set Code = 'MessagingStock.StartDateHelpText' where Code = 'AWBMessagingStock.StartDateHelpText'
update TextCodes set Code = 'MessagingStock.RemainingHelpText' where Code = 'AWBMessagingStock.RemainingHelpText'
update TextCodes set Code = 'MessagingStock.Q.AllAWBMessagingStocks' where Code = 'AWBMessagingStock.Q.AllAWBMessagingStocks'
update TextCodes set Code = 'MessagingStock.Q.1-42530' where Code = 'AWBMessagingStock.Q.1-42530'
update TextCodes set Code = 'MessagingStock.NotesHelpText' where Code = 'AWBMessagingStock.NotesHelpText'
update TextCodes set Code = 'MessagingStock.Q.1-42387' where Code = 'AWBMessagingStock.Q.1-42387'
update TextCodes set Code = 'MessagingStock.Q.1-42388' where Code = 'AWBMessagingStock.Q.1-42388'
update TextCodes set Code = 'MessagingStock.TotalPriceHelpText' where Code = 'AWBMessagingStock.TotalPriceHelpText'
update TextCodes set Code = 'MessagingStock.UpdateDateHelpText' where Code = 'AWBMessagingStock.UpdateDateHelpText'
update TextCodes set Code = 'MessagingStock.UpdatedByUserIdHelpText' where Code = 'AWBMessagingStock.UpdatedByUserIdHelpText'
delete from TextCodes where Code = 'AccountingSetting.CH.IsChronologicalDates'
delete from TextCodes where Code = 'Customer.CH.BillToNameListLable'
delete from TextCodes where Code = 'Participant.AccountingCardHelpText'
delete from ObjectFields where FieldName = 'IsChronologicalDates'
delete from TextCodes where Code like '%IsChronologicalDates%'
 delete from textcodes where code=  'Participant.CH.AccountingCardListLable'


