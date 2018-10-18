
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'OpenAmountInInvoiceCurrency' AND ObjectTableId = (select Id from ObjectTables where Name = 'APInvoice'))
go

delete from ObjectFields where FieldName = 'OpenAmountInInvoiceCurrency' AND ObjectTableId = (select Id from ObjectTables where Name = 'APInvoice')
go

delete from TextCodes where Code like '%APInvoice%OpenAmountInInvoiceCurrency%'
go

