
create table [OpportunityProductTypes] (
    [Code] [varchar](2) not null,
    [Name] [nvarchar](60) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [OpportunityProductPeriods] (
    [Code] [varchar](2) not null,
    [Name] [nvarchar](60) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [OpportunityProducts] (
    [OpportunityId] [varchar](15) not null,
    [OpportunityProductTypeCode] [varchar](2) not null,
    [Tenant] [int] not null,
	[Notes] [nvarchar](250) null,
	[OpportunityProductPeriodCode] [varchar](2) null,
	[ChargeableWeight] [decimal] null,
	[TEU] [decimal] null,
	[NumberOfShipments] [int] null,
	[Revenue] [decimal] null,
	[QuoteId] [varchar](15) null,
    primary key ([OpportunityId], [OpportunityProductTypeCode])
);
GO

alter table [OpportunityProducts] add constraint [OpportunityProduct_Opportunity] foreign key ([OpportunityId]) references [Opportunities]([Id]);
go

alter table [OpportunityProducts] add constraint [OpportunityProduct_OpportunityProductType] foreign key ([OpportunityProductTypeCode]) references [OpportunityProductTypes]([Code]);
go

alter table [OpportunityProducts] add constraint [OpportunityProduct_OpportunityProductPeriod] foreign key ([OpportunityProductPeriodCode]) references [OpportunityProductPeriods]([Code]);
go

