alter table [Customs].[SupplierInvoices] drop constraint [SupplierInvoice_PreferenceDocumentType]
go

alter table [Customs].[supplierinvoices] alter column PreferenceDocumentTypeCode varchar(3)
go
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_TradeAgreement] foreign key ([PreferenceDocumentTypeCode]) references [Customs].[TradeAgreements]([Code]);
go