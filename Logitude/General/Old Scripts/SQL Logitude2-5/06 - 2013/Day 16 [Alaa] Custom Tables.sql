create table [Customs].[DeclarationTaxes] (
    [DeclarationId] [varchar](15) not null,
    [TaxTypeCode] [varchar](3) not null,
    [TotalAmount] [decimal](18, 2) null,
    [DeferredTaxAmount] [decimal](18, 2) null,
    [Tenant] [int] not null,
    primary key ([DeclarationId], [TaxTypeCode])
);

create table [Customs].[ParagraphTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[SupplierInvoiceItemsTaxes] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [LineNumber] [int] not null,
    [TaxTypeCode] [varchar](3) not null,
    [Tenant] [int] not null,
    [TradeAgreementTypeCode] [varchar](3) null,
    [TaxRate] [decimal](18, 2) null,
    [TaxBaseAmount] [decimal](18, 2) null,
    [TaxAmount] [decimal](18, 2) null,
    [DeferedTaxAmount] [decimal](18, 2) null,
    [DefinedPerUnitMeasure] [decimal](18, 2) null,
    [AlternateDefinedPerUnitMeasure] [decimal](18, 2) null,
    [DefinedPerUnitQuantity] [decimal](18, 2) null,
    [AlternateDefinedPerUnitQuantity] [decimal](18, 2) null,
    [MeasurementUnitCode] [varchar](3) null,
    [AlternateMeasurementUnitCode] [varchar](3) null,
    [TradeLevyNumber] [varchar](9) null,
    [TotalBtlCoverageNIS] [decimal](18, 2) null,
    [AlternateRate] [decimal](18, 2) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [LineNumber], [TaxTypeCode])
);

create table [Customs].[TradeAgreements] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[MeasurmentUnits] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[ModificationAndDiscountTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName ] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[SupplierInvoiceModifications] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [TypeCode] [varchar](3) not null,
    [Tenant] [int] null,
    [CurrencyTypeCode] [varchar](3) null,
    [Amount] [decimal](18, 2) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [TypeCode])
);

create table [Customs].[SupplierInvoiceItemsModifications] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [LineNumber] [int] not null,
    [TypeCode] [varchar](3) not null,
    [Tenant] [int] not null,
    [CurrencyTypeCode] [varchar](3) null,
    [Amount] [decimal](18, 2) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [LineNumber], [TypeCode])
);


create table [Customs].[SupplierInvioceItemsCertificates] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [LineNumber] [int] not null,
    [ItemCertificateCounterKey] [int] not null,
    [CertificateNumber] [varchar](35) null,
    [Tenant] [int] not null,
    [ConfirmationTypeCode] [varchar](4) null,
    [CertificateExemptionTypeCode] [varchar](3) null,
    [AttachmentTypeCode] [varchar](3) null,
    [LICENSE_TYPE] [varchar](4) null,
    [CustomsAttachmentID] [varchar](35) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [LineNumber], [ItemCertificateCounterKey])
);

create table [Customs].[ConfirmationTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](1000) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[AttachmentTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[CertificateExemptionTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[SupplierInvoiceItemsConnectedDeclarations] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [InvoiceItemLineNumber] [int] not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [DeclarationNumber] [varchar](35) null,
    [SEQ_NUMItemSequence] [int] null,
    [DeclarationTypeCode] [varchar](3) null,
    [AccountNumberInvoiceNumber] [int] null,
    [Quantity] [int] null,
    primary key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber], [LineNumber])
);


alter table [Customs].[DeclarationTaxes] add constraint [DeclarationTax_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
go

alter table [Customs].[DeclarationTaxes] add constraint [DeclarationTax_ParagraphType] foreign key ([TaxTypeCode]) references [Customs].[ParagraphTypes]([Code]);
go

alter table [Customs].[SupplierInvoiceItemsTaxes] add constraint [SupplierInvoiceItemsTax_ParagraphType] foreign key ([TaxTypeCode]) references [Customs].[ParagraphTypes]([Code]);
go

alter table [Customs].[SupplierInvoiceItemsTaxes] add constraint [SupplierInvoiceItemsTax_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [LineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
go

alter table [Customs].[SupplierInvoiceItemsTaxes] add constraint [SupplierInvoiceItemsTax_TradeAgreement] foreign key ([TradeAgreementTypeCode]) references [Customs].[TradeAgreements]([Code]);
go

alter table [Customs].[SupplierInvoiceItemsTaxes] add constraint [SupplierInvoiceItemsTax_AlternateMeasurmentUnit] foreign key ([AlternateMeasurementUnitCode]) references [Customs].[MeasurmentUnits]([Code]);
go

alter table [Customs].[SupplierInvoiceItemsTaxes] add constraint [SupplierInvoiceItemsTax_MeasurmentUnit] foreign key ([MeasurementUnitCode]) references [Customs].[MeasurmentUnits]([Code]);
go

alter table [Customs].[SupplierInvoiceModifications] add constraint [SupplierInvoiceModification_ModificationAndDiscountType] foreign key ([TypeCode]) references [Customs].[ModificationAndDiscountTypes]([Code]);
go


alter table [Customs].[SupplierInvoiceModifications] add constraint [SupplierInvoiceModification_CurrencyType] foreign key ([CurrencyTypeCode]) references [Customs].[CurrencyTypes]([Code]);
go

alter table [Customs].[SupplierInvoiceModifications] add constraint [SupplierInvoiceModification_ModificationAndDiscountType] foreign key ([TypeCode]) references [Customs].[ModificationAndDiscountTypes]([Code]);
go

alter table [Customs].[SupplierInvoiceModifications] add constraint [SupplierInvoiceModification_SupplierInvoice] foreign key ([DeclarationId], [InvoiceCounterKey]) references [Customs].[SupplierInvoices]([DeclarationId], [InvoiceCounterKey]);
go


alter table [Customs].[SupplierInvoiceItemsModifications] add constraint [SupplierInvoiceItemsModification_CurrencyType] foreign key ([CurrencyTypeCode]) references [Customs].[CurrencyTypes]([Code]);
go

alter table [Customs].[SupplierInvoiceItemsModifications] add constraint [SupplierInvoiceItemsModification_ModificationAndDiscountType] foreign key ([TypeCode]) references [Customs].[ModificationAndDiscountTypes]([Code]);
go

alter table [Customs].[SupplierInvoiceItemsModifications] add constraint [SupplierInvoiceItemsModification_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [LineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
go

alter table [Customs].[SupplierInvioceItemsCertificates] add constraint [SupplierInvioceItemsCertificate_AttachmentType] foreign key ([AttachmentTypeCode]) references [Customs].[AttachmentTypes]([Code]);
go

alter table [Customs].[SupplierInvioceItemsCertificates] add constraint [SupplierInvioceItemsCertificate_CertificateExemptionType] foreign key ([CertificateExemptionTypeCode]) references [Customs].[CertificateExemptionTypes]([Code]);
go

alter table [Customs].[SupplierInvioceItemsCertificates] add constraint [SupplierInvioceItemsCertificate_ConfirmationType] foreign key ([ConfirmationTypeCode]) references [Customs].[ConfirmationTypes]([Code]);
go

alter table [Customs].[SupplierInvioceItemsCertificates] add constraint [SupplierInvioceItemsCertificate_LicenseConfirmationType] foreign key ([LICENSE_TYPE]) references [Customs].[ConfirmationTypes]([Code]);
go

alter table [Customs].[SupplierInvioceItemsCertificates] add constraint [SupplierInvioceItemsCertificate_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [LineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
go

alter table [Customs].[SupplierInvoiceItemsConnectedDeclarations] add constraint [SupplierInvoiceItemsConnectedDeclaration_LeadDocumentType] foreign key ([DeclarationTypeCode]) references [Customs].[LeadDocumentTypes]([Code]);
go

alter table [Customs].[SupplierInvoiceItemsConnectedDeclarations] add constraint [SupplierInvoiceItemsConnectedDeclaration_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
go

