

delete from ARInvoiceStatus where Code = 'PR' OR Code = 'AC'
go

alter table APInvoiceLines drop DF__APInvoice__Excha__7C8F6DA6
go

alter table APInvoiceLines drop column ExchangeRateDate
go

delete from ObjectFields where FieldName = 'ExchangeRateDate' AND ObjectTableId = (Select Id from ObjectTables where Name = 'APInvoiceLine')
go

delete from TextCodes where Code like '%APInvoiceLine%ExchangeRateDate%'
go