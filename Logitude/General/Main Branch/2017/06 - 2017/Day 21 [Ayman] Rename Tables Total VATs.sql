
-- Run this then update Tenant 0
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'QuoteTotalVAT')
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'APInvoiceTotalVAT')
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoiceTotalVAT')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'QuoteTotalVAT')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'APInvoiceTotalVAT')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoiceTotalVAT')
delete from ObjectTables where Name = 'QuoteTotalVAT'
delete from ObjectTables where Name = 'APInvoiceTotalVAT'
delete from ObjectTables where Name = 'ARInvoiceTotalVAT'



