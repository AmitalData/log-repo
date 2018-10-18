create table [Customs].[SupplierInvoiceItemsTaxesModifications] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [LineNumber] [int] not null,
    [TaxTypeCode] [varchar](3) not null,
    [TypeCode] [varchar](3) not null,
    [Tenant] [int] not null,
    [CurrencyTypeCode] [varchar](3) null,
    [Amount] [decimal](16, 2) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [LineNumber], [TaxTypeCode], [TypeCode])
);

alter table [Customs].[SupplierInvoiceItemsTaxesModifications] add constraint [SupplierInvoiceItemsTaxesModification_CurrencyType] foreign key ([CurrencyTypeCode]) references [Customs].[CurrencyTypes]([Code]);
alter table [Customs].[SupplierInvoiceItemsTaxesModifications] add constraint [SupplierInvoiceItemsTaxesModification_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[SupplierInvoiceItemsTaxesModifications] add constraint [SupplierInvoiceItemsTaxesModification_ModificationAndDiscountType] foreign key ([TypeCode]) references [Customs].[ModificationAndDiscountTypes]([Code]);
alter table [Customs].[SupplierInvoiceItemsTaxesModifications] add constraint [SupplierInvoiceItemsTaxesModification_TaxType] foreign key ([TaxTypeCode]) references [Customs].[ParagraphTypes]([Code]);
