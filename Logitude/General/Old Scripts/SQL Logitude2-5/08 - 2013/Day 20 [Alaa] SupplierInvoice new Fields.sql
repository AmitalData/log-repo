alter table [Customs].[SupplierInvoices] drop column TotalInsuranceInInvoiceCurrency , TotalInsuranceInNIS
go
alter table [Customs].[SupplierInvoices] add  InsruanceCurrencyTypeCode varchar(3) , InsuranceAmount decimal(16,2)
go
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_InsruanceCurrencyType] foreign key ([InsruanceCurrencyTypeCode]) references [Customs].[CurrencyTypes]([Code]);
go