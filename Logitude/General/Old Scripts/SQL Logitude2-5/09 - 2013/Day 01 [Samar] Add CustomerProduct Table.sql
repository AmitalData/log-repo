

IF OBJECT_ID(N'[dbo].[CustomerProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerProducts];
GO

IF OBJECT_ID(N'[dbo].[ProductTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductTypes];
GO

IF OBJECT_ID(N'[dbo].[ProductPeriods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductPeriods];
GO

create table [ProductTypes] (
    [Code] [varchar](2) not null,
    [Name] [nvarchar](60) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [ProductPeriods] (
    [Code] [varchar](2) not null,
    [Name] [nvarchar](60) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
GO

create table [CustomerProducts] (
    [CustomerId] [varchar](15) not null,
    [ProductTypeCode] [varchar](2) not null,
    [Tenant] [int] not null,
	[Notes] [nvarchar](250) null,
	[ProductPeriodCode] [varchar](2) null,
	[ActualChargeableWeight] [decimal] null,
	[PotentialChargeableWeight] [decimal] null,
	[CommitmentChargeableWeight] [decimal] null,
	[ActualTEU] [decimal] null,
	[PotentialTEU] [decimal] null,
	[CommitmentTEU] [decimal] null,
	[ActualNumberOfShipments] [int] null,
	[PotentialNumberOfShipments] [int] null,
	[CommitmentNumberOfShipments] [int] null,
    primary key ([CustomerId], [ProductTypeCode])
);
GO

alter table [CustomerProducts] add constraint [CustomerProduct_Customer] foreign key ([CustomerId]) references [Customers]([Id]);
go

alter table [CustomerProducts] add constraint [CustomerProduct_ProductPeriod] foreign key ([ProductPeriodCode]) references [ProductPeriods]([Code]);
go

alter table [CustomerProducts] add constraint [CustomerProduct_ProductType] foreign key ([ProductTypeCode]) references [ProductTypes]([Code]);
go

