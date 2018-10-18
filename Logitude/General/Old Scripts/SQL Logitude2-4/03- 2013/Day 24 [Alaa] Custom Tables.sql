drop table Customs.PhysicalChecks
go
drop table Customs.Declarations
go
drop table Customs.CargoIdentifireTypes
go
drop table Customs.CheckSites
go
drop table Customs.CustomEntityTypes
go
drop table Customs.InitiatorTypes
go
drop table Customs.PhysicalCheckOperations
go
drop table Customs.PhysicalCheckStatusMessages
go
drop table Customs.QueueTypes
go
drop table Customs.Sites
go
drop table Customs.StorageSites
go
drop table Customs.CheckRepresentativeTypes
go
drop table Customs.CheckEntityTypes
go
drop table Customs.CheckQueueTypes
go



if (schema_id(N'Customs') is null) exec(N'create schema [Customs]');
create table [Customs].[CargoIdentifireTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[CheckEntityTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [varchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[CheckQueueTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [varchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[CheckRepresentativeTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [varchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[Declarations] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [CustomFileNo] [varchar](12) not null,
    [DeclarationNumber] [varchar](35) not null,
    [CustomerId] [varchar](15) not null,
    [ImporterNumber] [varchar](9) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
create table [Customs].[PhysicalChecks] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [OperationCode] [varchar](2) not null,
    [StatusMessageCode] [varchar](2) not null,
    [CheckId] [varchar](9) not null,
    [CargoTypeCode] [varchar](4) null,
    [InitiatorTypeCode] [varchar](4) null,
    [ContainerNubmer] [varchar](15) not null,
    [ImporterNumber] [varchar](9) null,
    [StorageSiteCode] [varchar](4) null,
    [CheckSiteCode] [varchar](4) null,
    [OpenDate] [datetime] not null,
    [LimitDate] [datetime] not null,
    [QueueTypeCode] [varchar](4) null,
    [CargoIdentifierTypeCode] [varchar](4) null,
    [CargoIdentifierKey1] [varchar](35) not null,
    [CargoIdentifierKey2] [varchar](35) not null,
    [CargoIdentifierKey3] [varchar](35) not null,
    [RowNumber] [nvarchar](max) null,
    [CheckEssence] [nvarchar](255) not null,
    [DeclarationId] [varchar](15) not null,
    [IsClosed] [bit] not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
create table [Customs].[PhysicalCheckOperations] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [varchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[PhysicalCheckStatusMessages] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [varchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[Sites] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) not null,
    [LocalName] [varchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CargoIdentifireType] foreign key ([CargoIdentifierTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckEntityType] foreign key ([CargoTypeCode]) references [Customs].[CheckEntityTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckQueueType] foreign key ([QueueTypeCode]) references [Customs].[CheckQueueTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckRepresentativeType] foreign key ([InitiatorTypeCode]) references [Customs].[CheckRepresentativeTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckSite] foreign key ([CheckSiteCode]) references [Customs].[Sites]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]) on delete cascade;
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_StorageSite] foreign key ([StorageSiteCode]) references [Customs].[Sites]([Code]);
