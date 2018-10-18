create table [Customs].[Deficits] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [NotificationTypeCode] [varchar](2) null,
    [DebtNotificationNumber] [varchar](9) null,
    [ProductionDate] [datetime] null,
    [DebtNotificationReason] [varchar](512) null,
    [RealesGoodsDescription] [varchar](512) null,
    [ValidityDateTo] [datetime] null,
    primary key ([Id])
);

create table [Customs].[Tapags] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [TapagNumber] [varchar](15) null,
    [LeadingFileNumber] [varchar](9) null,
    [TapagTypeCode] [varchar](1) null,
    [CustomerId] [varchar](15) null,
    [ImporterId] [varchar](15) null,
    [CustomsBranchCode] [varchar](17) null,
    [ProfessionUnitTypeCode] [varchar](3) null,
    [SpecializationTypeCode] [varchar](2) null,
    [CreateDate] [datetime] null,
    [FollowDate] [datetime] null,
    [ValidityDate] [datetime] null,
    [IsClosed] [bit] not null,
    primary key ([Id])
);

create table [Customs].[TapagTypes] (
    [Code] [varchar](1) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[SpecializationTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
create table [Customs].[NotificationTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[Deficits] add constraint [Deficit_NotificationType] foreign key ([NotificationTypeCode]) references [Customs].[NotificationTypes]([Code]);
alter table [Customs].[Tapags] add constraint [Tapag_CustomerCard] foreign key ([CustomerId]) references [dbo].[Cards]([Id]);
alter table [Customs].[Tapags] add constraint [Tapag_CustomsBranch] foreign key ([CustomsBranchCode]) references [Customs].[SiteLookups]([Code]);
alter table [Customs].[Tapags] add constraint [Tapag_Importer] foreign key ([ImporterId]) references [Customs].[Clients]([Id]);
alter table [Customs].[Tapags] add constraint [Tapag_ProfessionUnitType] foreign key ([ProfessionUnitTypeCode]) references [Customs].[OrganizationUnitTypes]([Code]);
alter table [Customs].[Tapags] add constraint [Tapag_SpecializationType] foreign key ([SpecializationTypeCode]) references [Customs].[SpecializationTypes]([Code]);
alter table [Customs].[Tapags] add constraint [Tapag_TapagType] foreign key ([TapagTypeCode]) references [Customs].[TapagTypes]([Code]);
alter table [Customs].[Tapags] add constraint [Deficit_Tapag] foreign key ([Id]) references [Customs].[Deficits]([Id]);