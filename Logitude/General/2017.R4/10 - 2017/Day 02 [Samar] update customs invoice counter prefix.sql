
delete from ShipmentReceivables where ARInvoiceLineId in (select Id from ARInvoiceLines where ARInvoiceId in (select Id from ARInvoices where ARInvoiceTypeCode = 'CA'))
delete from ARInvoiceLines where ARInvoiceId in (select Id from ARInvoices where ARInvoiceTypeCode = 'CA')
delete from ARInvoiceTotalVATs where ARInvoiceId in (select Id from ARInvoices where ARInvoiceTypeCode = 'CA')
delete from ARInvoiceEntities where ARInvoiceId in (select Id from ARInvoices where ARInvoiceTypeCode = 'CA')
delete from ARInvoices where ARInvoiceTypeCode = 'CA'

delete from ARInvoiceTypes where Code = 'CA'

update CounterDefinitions set Parameter1 = 'CI', Prefix = 'CI' where Parameter1 = 'CA'

update DocumentTypes set Code = '999CI' where Code = '999CA'
