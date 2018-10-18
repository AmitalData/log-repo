
create table [Ratings] (
    [Code] [varchar](1) not null,
    [Name] [nvarchar](60) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [ClosingReasons] (
    [Code] [varchar](2) not null,
    [Name] [nvarchar](60) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [Stages] (
	[Id] [varchar] (15) not null,
	[Tenant] [int] not null,
    [Code] [varchar](3) not null,
    [Name] [nvarchar](60) not null,
	[Probability] [int] not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
GO

create table [LeadSources](
	[Id] [varchar] (15) not null,
	[Tenant] [int] not null,
    [Code] [varchar](2) not null,
    [Name] [nvarchar](60) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
GO

create table [Opportunities] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [OwnerId] [varchar](15) not null,
    [Topic] [nvarchar](250) not null,
    [CustomerId] [varchar](15) not null,
    [LeadSourceId] [varchar](15) null,
    [ContactId] [varchar](15) null,
    [BudgetAmount] [Decimal] null,
    [CurrencyId] [varchar](15) null,
    [EstimatedClosingDate] [DateTime] null,
    [StageId] [varchar](15) not null,
    [Probability] [int] null,
    [EstimatedRevenue] [Decimal] null,
    [Description] [nvarchar](1000) null,
    [CreateDate] [DateTime] not null,
    [CreatedByUserId] [varchar](15) not null,
    [UpdateDate] [DateTime] not null,
    [UpdatedByUserId] [varchar](15) not null,
    [RatingCode] [varchar](1) null,
    [IsClosed] [bit] not null,
    [ClosingReasonCode] [varchar](2) null,
    [ActualClosingDate] [DateTime] null,
	[ClosingDescription] [nvarchar](1000) null,
    primary key ([Id])
);
GO

create table [Competitors](
	[Id] [varchar] (15) not null,
	[Tenant] [int] not null,
    [Name] [nvarchar](60) not null,
	[Website] [varchar](100) null,
    [Strengths] [nvarchar](1000) null,
	[Weaknesses] [nvarchar](1000) null,
	[Opportunity] [nvarchar](1000) null,
	[Threat] [nvarchar](1000) null,
	[AddressId] [varchar](15) null,
    primary key ([Id])
);
GO

create table [OpportunitiesCompetitors](
	[Id] [varchar] (15) not null,
	[Tenant] [int] not null,
    [OpportunityId] [varchar](15) not null,
    [CompetitorId] [varchar](15) not null,
    primary key ([Id])
);
GO

alter table [Opportunities] add constraint [Opportunity_Rating] foreign key ([RatingCode]) references [Ratings]([Code]);
go

alter table [Opportunities] add constraint [Opportunity_ClosingReason] foreign key ([ClosingReasonCode]) references [ClosingReasons]([Code]);
go

alter table [Opportunities] add constraint [Opportunity_Stage] foreign key ([StageId]) references [Stages]([Id]);
go

alter table [Opportunities] add constraint [Opportunity_LeadSource] foreign key ([LeadSourceId]) references [LeadSources]([Id]);
go

alter table [Opportunities] add constraint [Opportunity_OwnerUser] foreign key ([OwnerId]) references [Users]([Id]);
go

alter table [Opportunities] add constraint [Opportunity_Customer] foreign key ([CustomerId]) references [Customers]([Id]);
go

alter table [Opportunities] add constraint [Opportunity_Contact] foreign key ([ContactId]) references [Contacts]([Id]);
go

alter table [Opportunities] add constraint [Opportunity_Currency] foreign key ([CurrencyId]) references [Currencies]([Id]);
go

alter table [Opportunities] add constraint [Opportunity_CreatedByUser] foreign key ([CreatedByUserId]) references [Users]([Id]);
go

alter table [Opportunities] add constraint [Opportunity_UpdatedByUser] foreign key ([UpdatedByUserId]) references [Users]([Id]);
go

alter table [Competitors] add constraint [Competitor_Address] foreign key ([AddressId]) references [Addresses]([Id]);
go

alter table [OpportunitiesCompetitors] add constraint [OpportunitiesCompetitors_Opportunity] foreign key ([OpportunityId]) references [Opportunities]([Id]);
go

alter table [OpportunitiesCompetitors] add constraint [OpportunitiesCompetitors_Competitor] foreign key ([CompetitorId]) references [Competitors]([Id]);
go