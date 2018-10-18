create table [Customs].[SupplierInvoiceItemsDescriptions] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [InvoiceItemLineNumber] [int] not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [TypeCode] [varchar](3) null,
    [Description] [nvarchar](35) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber], [LineNumber])
);

create table [Customs].[SupplierInvoiceItemsProductIdentifications] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [InvoiceItemLineNumber] [int] not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [TypeCode] [varchar](25) null,
    [Identification] [varchar](3) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber], [LineNumber])
);

create table [Customs].[SupplierInvoiceItemsSerialNumbers] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [InvoiceItemLineNumber] [int] not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [TypeCode] [varchar](4) null,
    [SerialNumber] [varchar](25) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber], [LineNumber])
);

create table [Customs].[ConsignmentInternalTransitions] (
    [DeclarationId] [varchar](15) not null,
    [ConsignmentNumber] [int] not null,
    [SiteCode] [varchar](17) not null,
    [Tenant] [int] not null,
    primary key ([DeclarationId], [ConsignmentNumber], [SiteCode])
);

create table [Customs].[ProductIdentificationTypes] (
    [Code] [varchar](25) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[SupplierInvoiceItemsProductIdentifications] add constraint [SupplierInvoiceItemsProductIdentification_ProductIdentificationType] foreign key ([TypeCode]) references [Customs].[ProductIdentificationTypes]([Code]);
alter table [Customs].[SupplierInvoiceItemsProductIdentifications] add constraint [SupplierInvoiceItemsProductIdentification_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
alter table [Customs].[SupplierInvoiceItemsSerialNumbers] add constraint [SupplierInvoiceItemsSerialNumber_CargoIdentifireType] foreign key ([TypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
alter table [Customs].[SupplierInvoiceItemsSerialNumbers] add constraint [SupplierInvoiceItemsSerialNumber_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
alter table [Customs].[SupplierInvoiceItemsDescriptions] add constraint [SupplierInvoiceItemsDescription_ProductNameType] foreign key ([TypeCode]) references [Customs].[ProductNameTypes]([Code]);
alter table [Customs].[SupplierInvoiceItemsDescriptions] add constraint [SupplierInvoiceItemsDescription_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
alter table [Customs].[ConsignmentInternalTransitions] add constraint [ConsignmentInternalTransition_Consignment] foreign key ([DeclarationId], [ConsignmentNumber]) references [Customs].[Consignments]([DeclarationId], [ConsignmentNumber]);
