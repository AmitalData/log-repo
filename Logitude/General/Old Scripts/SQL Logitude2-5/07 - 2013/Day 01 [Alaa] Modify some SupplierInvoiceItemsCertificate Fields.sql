SP_RENAME '[Customs].[SupplierInvioceItemsCertificates].[ConfirmationTypeCode]', 'ReqConfirmationTypeCode', 'COLUMN';
GO
SP_RENAME '[Customs].[SupplierInvioceItemsCertificates].[LICENSE_TYPE]', 'ResConfirmationTypeCode', 'COLUMN';
GO

alter table [Customs].[SupplierInvioceItemsCertificates] drop constraint [SupplierInvioceItemsCertificate_ConfirmationType]
go 
alter table [Customs].[SupplierInvioceItemsCertificates] drop constraint [SupplierInvioceItemsCertificate_LicenseConfirmationType] 
go

alter table [Customs].[SupplierInvioceItemsCertificates] add constraint [SupplierInvioceItemsCertificate_RequestConfirmationType] foreign key ([ReqConfirmationTypeCode]) references [Customs].[ConfirmationTypes]([Code]);
go
alter table [Customs].[SupplierInvioceItemsCertificates] add constraint [SupplierInvioceItemsCertificate_ResponseConfirmationType] foreign key ([ResConfirmationTypeCode]) references [Customs].[ConfirmationTypes]([Code]);
go

delete from ObjectFields where FieldName='ConfirmationTypeCode'
go
delete from ObjectFields where FieldName='LICENSE_TYPE'
go

delete from TextCodes where code like '%ConfirmationTypeCode%' OR code like '%LICENSE_TYPE%'
go

