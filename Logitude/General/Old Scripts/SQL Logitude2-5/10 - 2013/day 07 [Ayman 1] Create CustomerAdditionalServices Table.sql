
begin transaction
begin

IF OBJECT_ID(N'[dbo].[CustomerAdditionalServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].CustomerAdditionalServices;

-- Creating table 'CustomerAdditionalServices'
CREATE TABLE [dbo].CustomerAdditionalServices (
    [CustomerId] [varchar](15) not null,
    [ProductTypeCode] [varchar](2) not null,
	[Tenant]int NOT NULL,
	primary key ([CustomerId],[ProductTypeCode])
);

alter table [dbo].[CustomerAdditionalServices] add constraint [CustomerAdditionalService_Customer] foreign key ([CustomerId]) references [dbo].[Customers]([Id]);
alter table [dbo].[CustomerAdditionalServices] add constraint [CustomerAdditionalService_ProductType] foreign key ([ProductTypeCode]) references [dbo].[ProductTypes]([Code]);

END
commit transaction