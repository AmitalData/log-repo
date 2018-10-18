create table [Customs].[SupplierInvoiceFreightAmounts] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [CurrencyTypeCode] [varchar](3) not null,
    [Tenant] [int] not null,
    [Amount] [decimal](16, 2) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [CurrencyTypeCode])
);

alter table [Customs].[SupplierInvoiceFreightAmounts] add constraint [SupplierInvoiceFreightAmount_CurrencyType] foreign key ([CurrencyTypeCode]) references [Customs].[CurrencyTypes]([Code]);
alter table [Customs].[SupplierInvoiceFreightAmounts] add constraint [SupplierInvoiceFreightAmount_SupplierInvoice] foreign key ([DeclarationId], [InvoiceCounterKey]) references [Customs].[SupplierInvoices]([DeclarationId], [InvoiceCounterKey]);
