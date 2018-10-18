
IF OBJECT_ID(N'[dbo].[CustomerProduct_ProductPeriod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerProducts] DROP CONSTRAINT [CustomerProduct_ProductPeriod];
GO

alter table [CustomerProducts] drop column [ProductPeriodCode]
go

alter table [CustomerProducts] drop column [ActualChargeableWeight]
go

alter table [CustomerProducts] drop column [ActualTEU]
go

alter table [CustomerProducts] drop column [ActualNumberOfShipments]
go
