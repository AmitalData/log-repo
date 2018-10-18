create table [Customs].[TariffCodeTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_TariffCodeType] foreign key ([TariffCode]) references [Customs].[TariffCodeTypes]([Code]);
