create table [Customs].[MeasureQualifier] (
    [Code] [varchar](4) not null,
    [LocalName] [nvarchar](100) null,
    [EnglishName] [varchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[SupplierInvoiceItemsQuantities] add constraint [SupplierInvoiceItemsQuantity_MeasureQualifier] foreign key ([MeasureQualifierCode]) references [Customs].[MeasureQualifier]([Code]);
