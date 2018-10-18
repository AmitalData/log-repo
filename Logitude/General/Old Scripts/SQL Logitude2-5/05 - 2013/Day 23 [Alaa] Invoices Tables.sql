create table [Customs].[SupplierInvoices] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [Tenant] [int] not null,
    [SequenceNumeric] [int] null,
    [InvoiceNumber] [varchar](35) null,
    [IssueDate] [datetime] not null,
    [AccountTypeCode] [varchar](3) null,
    [IsPreference] [bit] not null,
    [PreferenceDocumentTypeCode] [varchar](4) null,
    [PaymentTypeCode] [varchar](4) null,
    [InvoiceCurrencyTypeCode] [varchar](3) null,
    [InvoiceAmount] [decimal](18, 2) not null,
    [ActualPayedCurrencyTypeCode] [varchar](3) null,
    [ActualPayedAmount] [decimal](18, 2) not null,
    [VendorId] [varchar](15) null,
    [IncotermCode] [varchar](3) null,
    [IssueCountryCode] [varchar](2) null,
    [PaymentTermsCode] [varchar](17) null,
    [TotalFreightInInvoiceCurrency] [decimal](18, 2) not null,
    [TotalFreightInNIS] [decimal](18, 2) not null,
    [TotalInsuranceInInvoiceCurrency] [decimal](18, 2) not null,
    [TotalInsuranceInNIS] [decimal](18, 2) not null,
    [ExchangeRate] [decimal](18, 2) not null,
    primary key ([DeclarationId], [InvoiceCounterKey])
);
create table [Customs].[SupplierInvoiceItems] (
    [DeclarationId] [varchar](15) not null,
    [CounterKey] [int] not null,
    [LineNumber] [int] not null,
    [ItemCode] [varchar](15) null,
    [OriginCountryCode] [varchar](2) null,
    [TariffCode] [varchar](3) null,
    [ClassificationCode] [varchar](18) null,
    [DangerousClassificationCode] [varchar](18) null,
    [DangerousPackingGroupTypeCode] [varchar](3) null,
    [ItemPrice] [decimal](18, 2) not null,
    [NonCustomsItemPrice] [decimal](18, 2) not null,
    [WholeSaleItemPrice] [decimal](18, 2) not null,
    [PROCESS_TYPE] [varchar](7) null,
    [ManufactureIdentifier] [varchar](17) null,
    [CustomsBookTypeCode] [varchar](4) null,
    [TaxExemptCode] [varchar](15) null,
    [OptionalTamaPercentage] [decimal](18, 2) not null,
    [SalesTaxExemptionTypeCode] [varchar](4) null,
    [INV_LINE_CONN] [varchar](1024) null,
    primary key ([DeclarationId], [CounterKey], [LineNumber])
);

create table [Customs].[PaymentTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [varchar](100) null,
    [SearchFields] [varchar](1000) null,
    primary key ([Code])
);

create table [Customs].[SupplierInvoiceItemsQuantities] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [InvoiceItemLineNumber] [int] not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [MeasureQualifierCode] [varchar](4) null,
    [Quantity] [decimal](18, 2) not null,
    primary key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber], [LineNumber])
);

create table [Customs].[PreferenceDocumentTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [varchar](100) null,
    [SearchFields] [varchar](1000) null,
    primary key ([Code])
);

create table [Customs].[SalesTaxExemptionTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [varchar](100) null,
    [SearchFields] [varchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_CurrencyType] foreign key ([InvoiceCurrencyTypeCode]) references [Customs].[CurrencyTypes]([Code]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_IssueCountry] foreign key ([IssueCountryCode]) references [Customs].[Countries]([Code]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_PayedCurrencyType] foreign key ([ActualPayedCurrencyTypeCode]) references [Customs].[CurrencyTypes]([Code]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_PaymentTerm] foreign key ([PaymentTermsCode]) references [Customs].[PaymentTerms]([Code]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_TermsOfSaleType] foreign key ([IncotermCode]) references [Customs].[TermsOfSaleTypes]([Code]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_Vendor] foreign key ([VendorId]) references [Customs].[Vendors]([Id]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_CustomsBookType] foreign key ([CustomsBookTypeCode]) references [Customs].[CustomsBookTypes]([Code]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_DangerousGoodsPackingReq] foreign key ([DangerousPackingGroupTypeCode]) references [Customs].[DangerousGoodsPackingReqs]([Code]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_ItemGovernmentProcedureType] foreign key ([PROCESS_TYPE]) references [Customs].[ItemGovernmentProcedureTypes]([Code]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_OriginCountry] foreign key ([OriginCountryCode]) references [Customs].[Countries]([Code]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_SupplierInvoice] foreign key ([DeclarationId], [CounterKey]) references [Customs].[SupplierInvoices]([DeclarationId], [InvoiceCounterKey]);
alter table [Customs].[SupplierInvoiceItemsQuantities] add constraint [SupplierInvoiceItemsQuantity_PackageMeasureQualifier] foreign key ([MeasureQualifierCode]) references [Customs].[PackageMeasureQualifiers]([Code]);
alter table [Customs].[SupplierInvoiceItemsQuantities] add constraint [SupplierInvoiceItemsQuantity_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_PaymentType] foreign key ([PaymentTypeCode]) references [Customs].[PaymentTypes]([Code]);
alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_PreferenceDocumentType] foreign key ([PreferenceDocumentTypeCode]) references [Customs].[PreferenceDocumentTypes]([Code]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_SalesTaxExemptionType] foreign key ([SalesTaxExemptionTypeCode]) references [Customs].[SalesTaxExemptionTypes]([Code]);
