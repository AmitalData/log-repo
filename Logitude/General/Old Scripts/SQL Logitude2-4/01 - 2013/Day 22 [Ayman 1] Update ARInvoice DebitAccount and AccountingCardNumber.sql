

update ARInvoices set DebitAccount = AccountingCardNumber
go

alter table ARInvoices drop column AccountingCardNumber
go

delete from ObjectFields where FieldName = 'AccountingCardNumber' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')
go

delete from TextCodes where Code like '%AccountingCardNumber%' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')
go

