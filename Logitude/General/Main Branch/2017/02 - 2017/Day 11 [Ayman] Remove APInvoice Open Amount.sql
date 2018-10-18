

delete from ObjectFields where FieldName = 'OpenAmountInInvoiceCurrency' and ObjectTableId = (select Id from ObjectTables where Name = 'APInvoice')
delete from TextCodes where Code = 'APInvoice.F.OpenAmountInInvoiceCurrency' 
delete from TextCodes where Code = 'APInvoice.CH.OpenAmountInInvoiceCurrencyListLable' 
delete from TextCodes where Code = 'APInvoice.OpenAmountInInvoiceCurrencyHelpText' 