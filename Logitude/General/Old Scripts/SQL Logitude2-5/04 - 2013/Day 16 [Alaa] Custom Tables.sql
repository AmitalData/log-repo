create table [Customs].[Vendors] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [VendorNumber] [varchar](9) not null,
    [VendorTypeCode] [nvarchar](2) not null,
    [VendorName] [varchar](9) not null,
    [CountryCode] [varchar](2) not null,
    [SubCountryCode] [varchar](6) null,
    [CityName] [varchar](35) null,
    [MainAddressLine] [varchar](70) null,
    [PostalCode] [varchar](15) null,
    [DunsNumber] [varchar](9) null,
    [VATNumber] [varchar](15) null,
    [StatusCode] [varchar](2) null,
    [TransactionTypeID] [varchar](2) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);

create table [Customs].[VendorTypes] (
    [Code] [nvarchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[VendorCommunications] (
    [VendorId] [varchar](15) not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [CommunicationTypeCode] [varchar](2) not null,
    [CommunicationAddress] [varchar](2) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([VendorId], [LineNumber], [Tenant])
);

create table [Customs].[SubCountries] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [varchar](40) null,
    [SearchFields] [nvarchar](40) null,
    primary key ([Code])
);

create table [Customs].[Countries] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[CommunicationTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);