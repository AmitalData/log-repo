delete from ObjectFields where ObjectTableId= ( select id from objecttables where name='customs.client') and FieldName='LocalName'
delete from ObjectFields where ObjectTableId= ( select id from objecttables where name='customs.client') and FieldName='EnglishName'

delete from TextCodes where code='Customs.Client.F.LocalName'
delete from TextCodes where code='Customs.Client.LocalNameHelpText'
delete from TextCodes where code='Customs.Client.CH.LocalNameListLable'
delete from TextCodes where code='Customs.Client.F.EnglishName'
delete from TextCodes where code='Customs.Client.EnglishNameHelpText'
delete from TextCodes where code='Customs.Client.CH.EnglishNameListLable'

alter table [Customs].[Clients] drop column  localName, EnglishName

alter table [Customs].[Clients] add  
    [FullName] [varchar](55) default null,
    [ClientTypeSpecificCode] [varchar](6) default null,
    [IsActive] [bit]  not null default 0,
    [LocalFirstName] [nvarchar](15) default null,
    [LocalLastName] [nvarchar](19) default null,
    [LocalCorporationName] [nvarchar](55) default null,
    [EnglishFirstName] [varchar](41) default null,
    [EnglishLastName] [varchar](30) default null,
    [EnglishCorporationName] [varchar](55) default null,
    [BirthDate] [datetime] default null,
    [GenderCode] [varchar](2) default null,
    [DunsNumber] [varchar](9) default null,
    [PassportNumber] [varchar](15) default null,
    [PassportCountryCode] [varchar](2) default null,
    [PassportTypeCode] [varchar](2) default null,
    [PassportFirstName] [varchar](41) default null,
    [PassportLastName] [varchar](30) default null,
    [EnglishBirthPlace] [varchar](35) default null,
    [EnglishFatherName] [varchar](20) default null,
    [PassportExpirationDate] [datetime] default null,
    [PassportIssueDate] [datetime] default null;



	create table [Customs].[ClientAddresses] (
    [ClientId] [varchar](15) not null,
    [AddressId] [varchar](9) not null,
    [Tenant] [int] not null,
    [ContactStateCode] [varchar](2) null,
    [AddressTypeCode] [varchar](2) null,
    [AddressPurposeCode] [varchar](2) null,
    [IsPalestinianCity] [bit] not null,
    [IsHebrewAddress] [bit] not null,
    [BranchName] [nvarchar](25) null,
    [ContactIdentifier] [varchar](9) null,
    [ContactFirstName] [nvarchar](15) null,
    [ContactLastName] [nvarchar](19) null,
    [ContactRoleTypeCode] [varchar](2) null,
    [AuthorizedSignerPermit1] [varchar](2) null,
    [AuthorizedSignerPermit2] [varchar](2) null,
    [AuthorizedSignerPermit3] [varchar](2) null,
    [LocalCityCode] [varchar](4) null,
    [LocalSecondLine] [nvarchar](25) null,
    [LocalStreetName] [nvarchar](20) null,
    [LocalHouseLetter] [nvarchar](1) null,
    [LocalEntrance] [nvarchar](2) null,
    [EnglishCountryCode] [varchar](2) null,
    [EnglishSubCountryCode] [varchar](6) null,
    [EnglishCityName] [varchar](35) null,
    [EnglishMainAddressLine] [varchar](70) null,
    [EnglishPostalCode] [varchar](9) null,
    [CommunicationTypeCode] [varchar](2) null,
    [CommunicationAddress] [varchar](50) null,
    [LocalApartment] [decimal](18, 2) null,
    [LocalPOBox] [decimal](18, 2) null,
    [LocalPostalCode] [decimal](18, 2) null,
    [LocalHouseNumber] [decimal](18, 2) null,
    primary key ([ClientId], [AddressId])
);



create table [Customs].[CustomerTypeGenerals] (
    [Code] [varchar](6) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[Genders] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[PassportTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[AddressContactStates] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[AddressTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[AddressPurposes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[ContactRoleTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[AuthorizedSignerPermits] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[Cities] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

