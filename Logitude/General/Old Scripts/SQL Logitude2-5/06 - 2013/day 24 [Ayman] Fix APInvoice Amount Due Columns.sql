
update ObjectFields set DataTemplateName = 'APInvoiceAmountDueInLocalDataTemplate'
where FieldName = 'AmountDueInLocalCurrency' and ObjectTableId = (Select Id from ObjectTables where Name = 'APInvoice')
go

update ObjectFields set DataTemplateName = 'APInvoiceAmountDueInProfitDataTemplate'
where FieldName = 'AmountDueInProfitCurrency' and ObjectTableId = (Select Id from ObjectTables where Name = 'APInvoice')
go