--Use Main DataBase--

drop table Customs.PhysicalChecks
go
drop table Customs.Declarations
go
drop table Customs.GenericTables
go

if (schema_id(N'Customs') is null) exec(N'create schema [Customs]');
create table [Customs].[CargoIdentifireTypes] (
    [Code] [varchar](4) not null,
    [Name] [varchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[CheckSites] (
    [Code] [varchar](4) not null,
    [Name] [varchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[CheckEntityTypes] (
    [Code] [varchar](4) not null,
    [Name] [varchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[Declarations] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [FileNo] [varchar](10) not null,
    [CustomFileNo] [varchar](12) not null,
    [DeclerationId] [varchar](35) not null,
    [CustomerId] [varchar](15) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
create table [Customs].[CheckRepresentativeTypes] (
    [Code] [varchar](4) not null,
    [Name] [varchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[PhysicalChecks] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [OperationCode] [varchar](2) not null,
    [StatusMessage] [varchar](2) not null,
    [CheckId] [varchar](9) not null,
    [EntityTypeCode] [varchar](4) null,
    [CheckRepresentativeTypeCode] [varchar](4) null,
    [ContainerNubmer] [varchar](15) not null,
    [StorageSiteCode] [varchar](4) null,
    [CheckSiteCode] [varchar](4) null,
    [OpenDate] [datetime] not null,
    [LimitDate] [datetime] not null,
    [CheckQueueTypeCode] [varchar](4) null,
    [CargoIdentifierTypeCode] [varchar](4) null,
    [CargoIdentifierKey1] [varchar](35) not null,
    [CargoIdentifierKey2] [varchar](35) not null,
    [CargoIdentifierKey3] [varchar](35) not null,
    [RowNumber] [nvarchar](max) null,
    [CheckEssence] [nvarchar](255) not null,
    [TypeDestination] [varchar](2) not null,
    [DeclarationId] [varchar](15) not null,
    [IsClosed] [bit] not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
create table [Customs].[CheckQueueTypes] (
    [Code] [varchar](4) not null,
    [Name] [varchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[StorageSites] (
    [Code] [varchar](4) not null,
    [Name] [varchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CargoIdentifireType] foreign key ([CargoIdentifierTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckSite] foreign key ([CheckSiteCode]) references [Customs].[CheckSites]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckEntityType] foreign key ([EntityTypeCode]) references [Customs].[CheckEntityTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]) on delete cascade;
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckRepresentativeType] foreign key ([CheckRepresentativeTypeCode]) references [Customs].[CheckRepresentativeTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckQueueType] foreign key ([CheckQueueTypeCode]) references [Customs].[CheckQueueTypes]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_StorageSite] foreign key ([StorageSiteCode]) references [Customs].[StorageSites]([Code]);
