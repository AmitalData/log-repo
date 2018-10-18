
delete from QueryColumns 
where
QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice'))
AND
ObjectFieldId = (select Id from ObjectFields where FieldName = 'EntityReference' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice'))
GO

delete from ObjectFields where FieldName = 'EntityReference' and ObjectTableId= (select Id from ObjectTables where Name = 'ARInvoice')
go

delete from TextCodes where Code = 'ARInvoice.F.EntityReference'
delete from TextCodes where Code = 'ARInvoice.EntityReferenceHelpText'
delete from TextCodes where Code = 'ARInvoice.CH.EntityReferenceListLable'



