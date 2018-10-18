SP_RENAME '[Customs].[SupplierInvoiceItemsConnectedDeclarations].[SEQ_NUMItemSequence]' , 'ItemSequence', 'COLUMN';
GO
SP_RENAME '[Customs].[SupplierInvoiceItemsConnectedDeclarations].[AccountNumberInvoiceNumber]' , 'InvoiceNumber', 'COLUMN';
GO

delete from objectfields where FieldName='SEQ_NUMItemSequence'
delete from ObjectFields where FieldName = 'AccountNumberInvoiceNumber'

delete from TextCodes where code like '%SEQ_NUMItemSequence%' OR code like '%AccountNumberInvoiceNumber%'








