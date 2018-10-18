create table [Customs].[InvoiceTypes] (
    [Code] [varchar](3) not null,
    [LocalName] [nvarchar](100) null,
    [EnglishName] [varchar](40) not null,
    [SearchFields] [varchar](1000) null,
    primary key ([Code])
);