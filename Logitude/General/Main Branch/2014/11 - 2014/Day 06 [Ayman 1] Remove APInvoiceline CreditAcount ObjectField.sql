
delete from ObjectFields where FieldName = 'CreditAccount' and ObjectTableId = (select Id from ObjectTables where Name = 'APInvoiceline')

delete from TextCodes where Code like '%APInvoiceline%CreditAccount%'