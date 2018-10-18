
IF OBJECT_ID(N'[dbo].[CustomerProductActualDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerProductActualDatas];
GO

IF OBJECT_ID(N'[dbo].[CustomerProductLocationActualDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerProductLocationActualDatas];
GO

IF OBJECT_ID(N'[dbo].[CustomerProductLocations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerProductLocations];
GO

create table [dbo].[CustomerProductActualDatas] (
    [Year] [int] not null,
    [CustomerId] [varchar](15) not null,
    [ProductTypeCode] [varchar](2) not null,
    [Month] [int] not null,
    [Tenant] [int] not null,
    [TEU] [decimal](18, 2) null,
    [NumberOfShipments] [int] null,
    [ChargeableWeight] [decimal](18, 2) null,
    primary key ([CustomerId],[ProductTypeCode],[Month],[Year])
);

create table [dbo].[CustomerProductLocations] (
    [CountryId] [varchar](15) not null,
    [CustomerId] [varchar](15) not null,
    [ProductTypeCode] [varchar](2) not null,
    [Tenant] [int] not null,
    [PotentialTEU] [decimal](18, 2) null,
    [PotentialNumberOfShipments] [int] null,
    [PotentialChargeableWeight] [decimal](18, 2) null,
    [CommitmentTEU] [decimal](18, 2) null,
    [CommitmentNumberOfShipments] [int] null,
    [CommitmentChargeableWeight] [decimal](18, 2) null,
    primary key ([CustomerId],[ProductTypeCode],[CountryId])
);

create table [dbo].[CustomerProductLocationActualDatas] (
    [CountryId] [varchar](15) not null,
    [CustomerId] [varchar](15) not null,
    [ProductTypeCode] [varchar](2) not null,
    [Month] [int] not null,
    [Year] [int] not null,
    [Tenant] [int] not null,
    [TEU] [decimal](18, 2) null,
    [NumberOfShipments] [int] null,
    [ChargeableWeight] [decimal](18, 2) null,
    primary key ([CustomerId],[ProductTypeCode],[Month],[Year],[CountryId])
);

alter table [dbo].[CustomerProductActualDatas] add constraint [CustomerProductActualData_Customer] foreign key ([CustomerId]) references [dbo].[Customers]([Id]);
alter table [dbo].[CustomerProductActualDatas] add constraint [CustomerProductActualData_ProductType] foreign key ([ProductTypeCode]) references [dbo].[ProductTypes]([Code]);
alter table [dbo].[CustomerProductLocations] add constraint [CustomerProductLocation_Country] foreign key ([CountryId]) references [dbo].[Countries]([Id]);
alter table [dbo].[CustomerProductLocations] add constraint [CustomerProductLocation_Customer] foreign key ([CustomerId]) references [dbo].[Customers]([Id]);
alter table [dbo].[CustomerProductLocations] add constraint [CustomerProductLocation_ProductType] foreign key ([ProductTypeCode]) references [dbo].[ProductTypes]([Code]);
alter table [dbo].[CustomerProductLocationActualDatas] add constraint [CustomerProductLocationActualData_Country] foreign key ([CountryId]) references [dbo].[Countries]([Id]);
alter table [dbo].[CustomerProductLocationActualDatas] add constraint [CustomerProductLocationActualData_Customer] foreign key ([CustomerId]) references [dbo].[Customers]([Id]);
alter table [dbo].[CustomerProductLocationActualDatas] add constraint [CustomerProductLocationActualData_ProductType] foreign key ([ProductTypeCode]) references [dbo].[ProductTypes]([Code]);
