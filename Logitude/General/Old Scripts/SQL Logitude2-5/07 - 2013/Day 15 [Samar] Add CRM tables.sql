

IF OBJECT_ID(N'[dbo].[Activity_Status]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [Activity_Status];
GO

IF OBJECT_ID(N'[dbo].[FK_ActivityStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_ActivityStatus];
GO

IF OBJECT_ID(N'[dbo].[Activity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activity];
GO

IF OBJECT_ID(N'[dbo].[Status]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Status];
GO

IF OBJECT_ID(N'[dbo].[ActivityTimeType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActivityTimeType];
GO

IF OBJECT_ID(N'[dbo].[ActivityType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActivityType];
GO

IF OBJECT_ID(N'[dbo].[CallType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CallType];
GO

IF OBJECT_ID(N'[dbo].[Priority]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Priority];
GO

IF OBJECT_ID(N'[dbo].[Activities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activities];
GO

IF OBJECT_ID(N'[dbo].[ActivityTimeTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActivityTimeTypes];
GO

IF OBJECT_ID(N'[dbo].[ActivityTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActivityTypes];
GO

IF OBJECT_ID(N'[dbo].[CallTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CallTypes];
GO

IF OBJECT_ID(N'[dbo].[Priorities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Priorities];
GO


IF OBJECT_ID(N'[dbo].[ActivityStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActivityStatus];
GO



create table [ActivityTimeTypes] (
    [Code] [varchar](2) not null,
    [Name] [varchar](20) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [ActivityTypes] (
    [Code] [varchar](2) not null,
    [Name] [varchar](20) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [CallTypes] (
    [Code] [varchar](1) not null,
    [Name] [varchar](20) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [Priorities](
    [Code] [varchar](2) not null,
    [Name] [varchar](20) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [ActivityStatus](
    [Code] [varchar](1) not null,
    [Name] [varchar](20) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [Activities] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [ActivityTypeCode] [varchar](2) not null,
    [Subject] [nvarchar](255) not null,
    [DueDate] [DateTime] null,
    [PriorityCode] [varchar](2) null,
    [OwnerId] [varchar](15) not null,
    [StartDateTime] [DateTime] null,
    [EndDateTime] [DateTime] null,
    [Description] [nvarchar](500) null,
    [SenderId] [varchar](15) null,
    [RecipientId] [varchar](15) null,
    [ObjectTableId] [varchar](15) null,
    [EntityId] [varchar](15) null,
    [CallTypeCode] [varchar](1) null,
    [CallPurpose] [nvarchar](500) null,
    [CallDetails] [nvarchar](500) null,
    [CallResult] [nvarchar](500) null,
    [PhoneNumber] [varchar](20) null,
    [Duration] [int] null,
    [Notes] [nvarchar](500) null,
    [CreatedByUserId] [varchar](15) not null,
    [CreateDate] [DateTime] not null,
    [ActivityStatusCode] [varchar](1) not null,
    [UpdatedByUserId] [varchar](15) not null,
    [UpdateDate] [DateTime] not null,
    [BranchId] [varchar](15) not null,
	[AllDayEvent] [Bit] not null,
	[ActivityTimeTypeCode] [varchar](2) null,
	[Location] [nvarchar](255) null,
    primary key ([Id])
);
GO

alter table [Activities] add constraint [Activity_ActivityTimeType] foreign key ([ActivityTimeTypeCode]) references [ActivityTimeTypes]([Code]);
go

alter table [Activities] add constraint [Activity_ActivityType] foreign key ([ActivityTypeCode]) references [ActivityTypes]([Code]);
go

alter table [Activities] add constraint [Activity_CallType] foreign key ([CallTypeCode]) references [CallTypes]([Code]);
go

alter table [Activities] add constraint [Activity_Priority] foreign key ([PriorityCode]) references [Priorities]([Code]);
go

alter table [Activities] add constraint [Activity_ActivityStatus] foreign key ([ActivityStatusCode]) references [ActivityStatus]([Code]);
go

alter table [Activities] add constraint [Activity_Owner] foreign key ([OwnerId]) references [Users]([Id]);
go

alter table [Activities] add constraint [Activity_SenderContact] foreign key ([SenderId]) references [Contacts]([Id]);
go

alter table [Activities] add constraint [Activity_RecipientContact] foreign key ([RecipientId]) references [Contacts]([Id]);
go

alter table [Activities] add constraint [Activity_ObjectTable] foreign key ([ObjectTableId]) references [ObjectTables]([Id]);
go

alter table [Activities] add constraint [Activity_CreatedByUser] foreign key ([CreatedByUserId]) references [Users]([Id]);
go

alter table [Activities] add constraint [Activity_UpdatedByUser] foreign key ([UpdatedByUserId]) references [Users]([Id]);
go

alter table [Activities] add constraint [Activity_Branch] foreign key ([BranchId]) references [Branches]([Id]);
go


