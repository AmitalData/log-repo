update ObjectFields 
set DataTypeCode = 'nText' 
where FieldName = 'PrintNotes' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')