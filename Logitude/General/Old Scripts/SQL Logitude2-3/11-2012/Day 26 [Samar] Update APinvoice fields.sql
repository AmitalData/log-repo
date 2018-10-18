delete from ObjectFields where FieldName = 'APInvoiceTypeCode' and ObjectTableId = (select Id from ObjectTables where Name = 'apinvoice')
delete from ObjectFields where FieldName = 'APInvoiceTypeName' and ObjectTableId = (select Id from ObjectTables where Name = 'apinvoice')

delete from textcodes where Code = 'apinvoice.f.APInvoiceTypeName'
delete from TextCodes where Code = 'apinvoice.f.APInvoiceTypeCode'