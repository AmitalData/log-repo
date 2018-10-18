create table [Customs].[CurrencyTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[CustomsBookTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[DangerousGoodsPackingReqs] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[ItemGovernmentProcedureTypes] (
    [Code] [varchar](7) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[PaymentTerms] (
    [Code] [varchar](17) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [varchar](1000) null,
    primary key ([Code])
);
create table [Customs].[ProductNameTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[TermsOfSaleTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);