
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/10/2011 09:50:08
-- Generated from EDMX file: C:\SimplogSourceCode\Pilot1\JustWebFreight\WebFreight.Web\QuoteModel\QuoteModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WebFreightBranch2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


IF OBJECT_ID(N'[dbo].[FK_QuoteShipmentType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuoteShipmentType];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientCardQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_ClientCardQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientAddressQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_ClientAddressQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientAbroadCardQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_ClientAbroadCardQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientAbroadAddressQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_ClientAbroadAddressQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientContactQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_ClientContactQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientAbroadContactQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_ClientAbroadContactQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_FromPortQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_FromPortQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_ToPortQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_ToPortQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_IncotermQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_IncotermQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesmanUserQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_SalesmanUserQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_OpenByUserQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_OpenByUserQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectionQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_DirectionQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_TransportModeQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_TransportModeQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_QuoteQuoteReceivable]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_QuoteQuoteReceivable];
GO
IF OBJECT_ID(N'[dbo].[FK_QuoteReceivableChargesType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_QuoteReceivableChargesType];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrencyQuoteReceivable]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_CurrencyQuoteReceivable];
GO
IF OBJECT_ID(N'[dbo].[FK_UpdatedByUserQuoteReceivable]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_UpdatedByUserQuoteReceivable];
GO
IF OBJECT_ID(N'[dbo].[FK_EntityStatusQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_EntityStatusQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_ChargesTypeMeasurement]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[ChargesTypes] DROP CONSTRAINT [FK_ChargesTypeMeasurement];
GO
IF OBJECT_ID(N'[dbo].[FK_ContainerChargesTypeMeasurement]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[ChargesTypes] DROP CONSTRAINT [FK_ContainerChargesTypeMeasurement];
GO
IF OBJECT_ID(N'[dbo].[FK_QuoteReceivableMeasurement]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_QuoteReceivableMeasurement];
GO

IF OBJECT_ID(N'[dbo].[FK_QuoteBranch]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuoteBranch];
GO
IF OBJECT_ID(N'[dbo].[FK_QuoteDepartment]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuoteDepartment];
GO


IF OBJECT_ID(N'[dbo].[FK_QuotePackageType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuotePackageType];
GO
IF OBJECT_ID(N'[dbo].[FK_PackageType2QuotePackageType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_PackageType2QuotePackageType];
GO
IF OBJECT_ID(N'[dbo].[FK_PackageType3QuotePackageType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_PackageType3QuotePackageType];
GO
IF OBJECT_ID(N'[dbo].[FK_PackageType4QuotePackageType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_PackageType4QuotePackageType];
GO
IF OBJECT_ID(N'[dbo].[FK_PackageType5QuotePackageType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_PackageType5QuotePackageType];
GO
IF OBJECT_ID(N'[dbo].[FK_QuoteQuoteType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuoteQuoteType];
GO
IF OBJECT_ID(N'[dbo].[FK_CostQuoteChargesMeasurement]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_CostQuoteChargesMeasurement];
GO
IF OBJECT_ID(N'[dbo].[FK_ContainerType1QuoteChargesMarkUpType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_ContainerType1QuoteChargesMarkUpType];
GO
IF OBJECT_ID(N'[dbo].[FK_ContainerType2QuoteChargesMarkUpType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_ContainerType2QuoteChargesMarkUpType];
GO
IF OBJECT_ID(N'[dbo].[FK_ContainerType3QuoteChargesMarkUpType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_ContainerType3QuoteChargesMarkUpType];
GO
IF OBJECT_ID(N'[dbo].[FK_ContainerType4QuoteChargesMarkUpType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_ContainerType4QuoteChargesMarkUpType];
GO
IF OBJECT_ID(N'[dbo].[FK_ContainerType5QuoteChargesMarkUpType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_ContainerType5QuoteChargesMarkUpType];
GO
IF OBJECT_ID(N'[dbo].[FK_QuoteChargesMarkUpType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_QuoteChargesMarkUpType];
GO
IF OBJECT_ID(N'[dbo].[FK_VendorQuoteChargesCard]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_VendorQuoteChargesCard];
GO


IF OBJECT_ID(N'[dbo].[FK_MaincarriageQuoteCard]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_MaincarriageQuoteCard];
GO
IF OBJECT_ID(N'[dbo].[FK_QuotePriceStepsQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuotePriceSteps] DROP CONSTRAINT [FK_QuotePriceStepsQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCardQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_CustomerCardQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerAddressQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_CustomerAddressQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContactQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_CustomerContactQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_QuoteQuoteCustomerType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuoteQuoteCustomerType];
GO
IF OBJECT_ID(N'[dbo].[FK_QuotePotentialCutomer]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuotePotentialCutomer];
GO

IF OBJECT_ID(N'[dbo].[FK_QuoteCurrency]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_QuoteCurrency];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrencyQuoteCharge]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[QuoteCharges] DROP CONSTRAINT [FK_CurrencyQuoteCharge];
GO
IF OBJECT_ID(N'[dbo].[FK_PotentialShipperQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_PotentialShipperQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_PotentialCustomerQuote]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[Quotes] DROP CONSTRAINT [FK_PotentialCustomerQuote];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Quotes]', 'U') IS NOT NULL
	DROP TABLE [dbo].[Quotes];
GO


IF OBJECT_ID(N'[dbo].[QuoteCharges]', 'U') IS NOT NULL
	DROP TABLE [dbo].[QuoteCharges];
GO



IF OBJECT_ID(N'[dbo].[QuotePriceSteps]', 'U') IS NOT NULL
	DROP TABLE [dbo].[QuotePriceSteps];
GO
IF OBJECT_ID(N'[dbo].[QuoteTypes]', 'U') IS NOT NULL
	DROP TABLE [dbo].[QuoteTypes];
GO
IF OBJECT_ID(N'[dbo].[MarkUpTypes]', 'U') IS NOT NULL
	DROP TABLE [dbo].[MarkUpTypes];
GO
IF OBJECT_ID(N'[dbo].[QuoteCustomerTypes]', 'U') IS NOT NULL
	DROP TABLE [dbo].[QuoteCustomerTypes];
GO
 

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Quotes'
CREATE TABLE [dbo].[Quotes] (
	[Id]varchar(15)   NOT NULL,
	[Tenant]int   NOT NULL,
	[QuoteNumber]varchar(15)   NOT NULL,
	[ShipperReference1]varchar(50)   NULL,
	[ShipperReference2]varchar(50)   NULL,
	[ConsigneeReference1]varchar(50)   NULL,
	[ConsigneeReference2]varchar(50)   NULL,
	[OpenDate]datetime   NOT NULL,
	[Notes]nvarchar(250)   NULL,
	[DescriptionOfGoods]varchar(512)   NULL,
	[IsClosed]bit   NOT NULL,
	[ChargeableWeight]float   NULL,
	[GrossWeight]float   NULL,
	[LastModified]TIMESTAMP   NOT NULL,
	[Field1]nvarchar(250)   NULL,
	[Field2]nvarchar(250)   NULL,
	[Field3]nvarchar(250)   NULL,
	[Field4]nvarchar(250)   NULL,
	[Field5]nvarchar(250)   NULL,
	[Field6]nvarchar(250)   NULL,
	[Field7]nvarchar(250)   NULL,
	[Field8]nvarchar(250)   NULL,
	[Field9]nvarchar(250)   NULL,
	[Field10]nvarchar(250)   NULL,
	[DimensionsUnitCode]varchar(3)   NULL,
	[GrossWeightUnitCode]varchar(3)   NULL,
	[Volume]float   NULL,
	[NumberOfContainers]int   NULL,
	[NumberOfPackages]int   NULL,
	[Ratio]float   NULL,
	[VolumeUnitCode]varchar(3)   NULL,
	[ShipmentTypeId]varchar(4)   NULL,
	[ShipperId]varchar(15)   NULL,
	[ShipperAddressId]varchar(15)   NULL,
	[ConsigneeId]varchar(15)   NULL,
	[ConsigneeAddressId]varchar(15)   NULL,
	[ShipperContactId]varchar(15)   NULL,
	[ConsigneeContactId]varchar(15)   NULL,
	[FromPortId]varchar(15)   NOT NULL,
	[ToPortId]varchar(15)   NOT NULL,
	[IncotermId]varchar(15)   NULL,
	[SalesmanUserId]varchar(15)   NULL,
	[CreatedByUserId]varchar(15)   NOT NULL,
	[DirectionId]char(1)   NOT NULL,
	[TransportModeId]char(1)   NOT NULL,
	[IsDangerous]bit   NOT NULL,
	[ExpirationDays]int   NULL,
	[ExpirationDate]datetime   NULL,
	[VolumetricWeight]float   NULL,
	[EntityStatusId]varchar(15)   NOT NULL,
	[BranchId]varchar(15)   NULL,
	[DepartmentId]varchar(15)   NULL,
	[PackageType1Id]varchar(15)   NULL,
	[PackageType2Id]varchar(15)   NULL,
	[PackageType3Id]varchar(15)   NULL,
	[PackageType4Id]varchar(15)   NULL,
	[PackageType5Id]varchar(15)   NULL,
	[PackageType1Quantity]int   NULL,
	[PackageType2Quantity]int   NULL,
	[PackageType3Quantity]int   NULL,
	[PackageType4Quantity]int   NULL,
	[PackageType5Quantity]int   NULL,
	[IsByKG]bit   NOT NULL,
	[IsByContainer]bit   NOT NULL,
	[QuoteTypeCode]varchar(1)   NOT NULL,
	[EstimateProfit]float   NULL,
	[EstimateProfitEdited]bit   NOT NULL,
	[MinimumFreightCost]float   NULL,
	[MinimumFreightSale]float   NULL,
	[MainCarriageCarrierId]varchar(15)   NULL,
	[IsFreightBySteps]bit   NOT NULL,
	[OrderNumberOfPackages]int   NULL,
	[IsCancelled]bit   NOT NULL,
	[CustomerId]varchar(15)   NULL,
	[CustomerAddressId]varchar(15)   NULL,
	[CustomerContactId]varchar(15)   NULL,
	[CustomerReference]nvarchar(50)   NULL,
	[QuoteCustomerTypeCode]varchar(4)   NOT NULL,
	[PotentialCustomerId]varchar(15)   NULL,
	[CustomerName]nvarchar(100)   NOT NULL,
	[SaleCurrencyId]varchar(15)   NOT NULL,
	[ExchangeRate]float   NOT NULL,
	[IncludePickUp]bit   NOT NULL,
	[IncludeDelivery]bit   NOT NULL,
	[ShipperName]nvarchar(100)    NULL,
	[ConsigneeName]nvarchar(100)  NULL,
	[PickUpAddress]nvarchar(250)   NULL,
	[DeliveryAddress]nvarchar(250)   NULL,
	[PotentialShipperId]varchar(15)   NULL,
	[PotentialConsigneeId]varchar(15)   NULL,
	[IsFixedPrice]bit   NOT NULL,
	[SearchFields]nvarchar(1000)   NULL,
	[ChargeableWeightUnitCode] varchar(3) null,
	[FromAddressId]varchar(15)   NULL,
	[ToAddressId]varchar(15)   NULL,
);
GO






-- Creating table 'QuoteCharges'
CREATE TABLE [dbo].[QuoteCharges] (
	[Id]varchar(15)   NOT NULL,
	[Tenant]int   NOT NULL,
	[SaleQuantity]float   NULL,
	[SaleUnitPrice]float   NULL,
	[SaleTotalAmount]float   NULL,
	[SaleTotalAmountLocal]float   NULL,
	[Notes]nvarchar(250)   NULL,
	[SaleExchangeRate]float   NULL,
	[UpdateDate]datetime   NOT NULL,
	[ValueDate]datetime   NOT NULL,
	[QuoteId]varchar(15)   NOT NULL,
	[ChargesTypeId]varchar(15)   NOT NULL,
	[SaleCurrencyId]varchar(15)   NOT NULL,
	[UpdatedByUserId]varchar(15)   NOT NULL,
	[SaleMeasurementId]varchar(15)   NOT NULL,
	[SaleContainerType1UnitPrice]float   NULL,
	[SaleContainerType2UnitPrice]float   NULL,
	[SaleContainerType3UnitPrice]float   NULL,
	[SaleContainerType4UnitPrice]float   NULL,
	[SaleContainerType5UnitPrice]float   NULL,
	[CostMeasurementId]varchar(15)   NULL,
	[CostUnitPrice]float   NULL,
	[CostContainerType1UnitPrice]float   NULL,
	[CostContainerType2UnitPrice]float   NULL,
	[CostContainerType3UnitPrice]float   NULL,
	[CostContainerType4UnitPrice]float   NULL,
	[CostContainerType5UnitPrice]float   NULL,
	[ContainerType1MarkUpTypeCode]varchar(4)   NULL,
	[ContainerType2MarkUpTypeCode]varchar(4)   NULL,
	[ContainerType3MarkUpTypeCode]varchar(4)   NULL,
	[ContainerType4MarkUpTypeCode]varchar(4)   NULL,
	[ContainerType5MarkUpTypeCode]varchar(4)   NULL,
	[ContainerType1MarkUpValue]float   NULL,
	[ContainerType2MarkUpValue]float   NULL,
	[ContainerType3MarkUpValue]float   NULL,
	[ContainerType4MarkUpValue]float   NULL,
	[ContainerType5MarkUpValue]float   NULL,
	[MarkUpTypeCode]varchar(4)   NULL,
	[MarkUpValue]float   NULL,
	[IsAllIN]bit   NOT NULL,
	[VendorId]varchar(15)   NULL,
	[CostQuantity]float   NULL,
	[CostTotalAmount]float   NULL,
	[CostTotalAmountLocal]float   NULL,
	[SaleIsExchangeRateFixed]bit   NOT NULL,
	[CostExchangeRate]float   NOT NULL,
	[CostIsFixedRate]bit   NOT NULL,
	[CostCurrencyId]varchar(15)   NOT NULL,
	[CostMaxAmount]float   NULL,
	[CostMinAmount]float   NULL
);
GO









-- Creating table 'QuotePriceSteps'
CREATE TABLE [dbo].[QuotePriceSteps] (
	[Id]varchar(15)   NOT NULL,
	[Tenant]int   NOT NULL,
	[SaleUnitPrice]float   NULL,
	[Step]float   NOT NULL,
	[CostUnitPrice]float   NULL,
	[QuoteId]varchar(15)   NOT NULL,
	[MarkupValue]float   NULL
);
GO

-- Creating table 'QuoteTypes'
CREATE TABLE [dbo].[QuoteTypes] (
	[Code]varchar(1)   NOT NULL,
	[Name]varchar(40)   NOT NULL,
	[SearchFields]nvarchar(1000)    NULL,
);
GO

-- Creating table 'MarkUpTypes'
CREATE TABLE [dbo].[MarkUpTypes] (
	[Code]varchar(4)   NOT NULL,
	[Name]varchar(40)   NOT NULL,
	[SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating table 'QuoteCustomerTypes'
CREATE TABLE [dbo].[QuoteCustomerTypes] (
	[Code]varchar(4)   NOT NULL,
	[Name]varchar(40)   NOT NULL,
	[SearchFields]nvarchar(1000)   NULL
);
GO

--

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [PK_Quotes]
	PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating primary key on [Id] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [PK_QuoteCharges]
	PRIMARY KEY CLUSTERED ([Id] ASC);
GO



-- Creating primary key on [Id] in table 'QuotePriceSteps'
ALTER TABLE [dbo].[QuotePriceSteps]
ADD CONSTRAINT [PK_QuotePriceSteps]
	PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'QuoteTypes'
ALTER TABLE [dbo].[QuoteTypes]
ADD CONSTRAINT [PK_QuoteTypes]
	PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'MarkUpTypes'
ALTER TABLE [dbo].[MarkUpTypes]
ADD CONSTRAINT [PK_MarkUpTypes]
	PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'QuoteCustomerTypes'
ALTER TABLE [dbo].[QuoteCustomerTypes]
ADD CONSTRAINT [PK_QuoteCustomerTypes]
	PRIMARY KEY CLUSTERED ([Code] ASC);
GO



-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------




-- Creating foreign key on [ShipmentTypeId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuoteShipmentType]
	FOREIGN KEY ([ShipmentTypeId])
	REFERENCES [dbo].[ShipmentTypes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteShipmentType'
CREATE INDEX [IX_FK_QuoteShipmentType]
ON [dbo].[Quotes]
	([ShipmentTypeId]);
GO

-- Creating foreign key on [ShipperId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ClientCardQuote]
	FOREIGN KEY ([ShipperId])
	REFERENCES [dbo].[Cards]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientCardQuote'
CREATE INDEX [IX_FK_ClientCardQuote]
ON [dbo].[Quotes]
	([ShipperId]);
GO

-- Creating foreign key on [ShipperAddressId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ClientAddressQuote]
	FOREIGN KEY ([ShipperAddressId])
	REFERENCES [dbo].[Addresses]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientAddressQuote'
CREATE INDEX [IX_FK_ClientAddressQuote]
ON [dbo].[Quotes]
	([ShipperAddressId]);
GO

-- Creating foreign key on [ConsigneeId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ClientAbroadCardQuote]
	FOREIGN KEY ([ConsigneeId])
	REFERENCES [dbo].[Cards]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientAbroadCardQuote'
CREATE INDEX [IX_FK_ClientAbroadCardQuote]
ON [dbo].[Quotes]
	([ConsigneeId]);
GO

-- Creating foreign key on [ConsigneeAddressId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ClientAbroadAddressQuote]
	FOREIGN KEY ([ConsigneeAddressId])
	REFERENCES [dbo].[Addresses]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientAbroadAddressQuote'
CREATE INDEX [IX_FK_ClientAbroadAddressQuote]
ON [dbo].[Quotes]
	([ConsigneeAddressId]);
GO

-- Creating foreign key on [ShipperContactId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ClientContactQuote]
	FOREIGN KEY ([ShipperContactId])
	REFERENCES [dbo].[Contacts]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientContactQuote'
CREATE INDEX [IX_FK_ClientContactQuote]
ON [dbo].[Quotes]
	([ShipperContactId]);
GO

-- Creating foreign key on [ConsigneeContactId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ClientAbroadContactQuote]
	FOREIGN KEY ([ConsigneeContactId])
	REFERENCES [dbo].[Contacts]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientAbroadContactQuote'
CREATE INDEX [IX_FK_ClientAbroadContactQuote]
ON [dbo].[Quotes]
	([ConsigneeContactId]);
GO

-- Creating foreign key on [FromPortId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_FromPortQuote]
	FOREIGN KEY ([FromPortId])
	REFERENCES [dbo].[Ports]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FromPortQuote'
CREATE INDEX [IX_FK_FromPortQuote]
ON [dbo].[Quotes]
	([FromPortId]);
GO

-- Creating foreign key on [ToPortId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ToPortQuote]
	FOREIGN KEY ([ToPortId])
	REFERENCES [dbo].[Ports]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ToPortQuote'
CREATE INDEX [IX_FK_ToPortQuote]
ON [dbo].[Quotes]
	([ToPortId]);
GO

-- Creating foreign key on [IncotermId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_IncotermQuote]
	FOREIGN KEY ([IncotermId])
	REFERENCES [dbo].[Incoterms]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_IncotermQuote'
CREATE INDEX [IX_FK_IncotermQuote]
ON [dbo].[Quotes]
	([IncotermId]);
GO

-- Creating foreign key on [SalesmanUserId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_SalesmanUserQuote]
	FOREIGN KEY ([SalesmanUserId])
	REFERENCES [dbo].[Users]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesmanUserQuote'
CREATE INDEX [IX_FK_SalesmanUserQuote]
ON [dbo].[Quotes]
	([SalesmanUserId]);
GO

-- Creating foreign key on [OpenedByUserId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_OpenByUserQuote]
	FOREIGN KEY ([CreatedByUserId])
	REFERENCES [dbo].[Users]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OpenByUserQuote'
CREATE INDEX [IX_FK_OpenByUserQuote]
ON [dbo].[Quotes]
	([CreatedByUserId]);
GO

-- Creating foreign key on [DirectionId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_DirectionQuote]
	FOREIGN KEY ([DirectionId])
	REFERENCES [dbo].[Directions]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectionQuote'
CREATE INDEX [IX_FK_DirectionQuote]
ON [dbo].[Quotes]
	([DirectionId]);
GO

-- Creating foreign key on [TransportModeId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_TransportModeQuote]
	FOREIGN KEY ([TransportModeId])
	REFERENCES [dbo].[TransportModes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TransportModeQuote'
CREATE INDEX [IX_FK_TransportModeQuote]
ON [dbo].[Quotes]
	([TransportModeId]);
GO

-- Creating foreign key on [QuoteId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_QuoteQuoteReceivable]
	FOREIGN KEY ([QuoteId])
	REFERENCES [dbo].[Quotes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteQuoteReceivable'
CREATE INDEX [IX_FK_QuoteQuoteReceivable]
ON [dbo].[QuoteCharges]
	([QuoteId]);
GO

-- Creating foreign key on [ChargesTypeId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_QuoteReceivableChargesType]
	FOREIGN KEY ([ChargesTypeId])
	REFERENCES [dbo].[ChargesTypes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteReceivableChargesType'
CREATE INDEX [IX_FK_QuoteReceivableChargesType]
ON [dbo].[QuoteCharges]
	([ChargesTypeId]);
GO

-- Creating foreign key on [SaleCurrencyId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_CurrencyQuoteReceivable]
	FOREIGN KEY ([SaleCurrencyId])
	REFERENCES [dbo].[Currencies]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrencyQuoteReceivable'
CREATE INDEX [IX_FK_CurrencyQuoteReceivable]
ON [dbo].[QuoteCharges]
	([SaleCurrencyId]);
GO

-- Creating foreign key on [UpdatedByUserId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_UpdatedByUserQuoteReceivable]
	FOREIGN KEY ([UpdatedByUserId])
	REFERENCES [dbo].[Users]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UpdatedByUserQuoteReceivable'
CREATE INDEX [IX_FK_UpdatedByUserQuoteReceivable]
ON [dbo].[QuoteCharges]
	([UpdatedByUserId]);
GO

-- Creating foreign key on [EntityStatusId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_EntityStatusQuote]
	FOREIGN KEY ([EntityStatusId])
	REFERENCES [dbo].[EntityStatus]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EntityStatusQuote'
CREATE INDEX [IX_FK_EntityStatusQuote]
ON [dbo].[Quotes]
	([EntityStatusId]);
GO


-- Creating foreign key on [SaleMeasurementId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_QuoteReceivableMeasurement]
	FOREIGN KEY ([SaleMeasurementId])
	REFERENCES [dbo].[Measurements]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteReceivableMeasurement'
CREATE INDEX [IX_FK_QuoteReceivableMeasurement]
ON [dbo].[QuoteCharges]
	([SaleMeasurementId]);
GO


-- Creating foreign key on [BranchId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuoteBranch]
	FOREIGN KEY ([BranchId])
	REFERENCES [dbo].[Branches]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteBranch'
CREATE INDEX [IX_FK_QuoteBranch]
ON [dbo].[Quotes]
	([BranchId]);
GO

-- Creating foreign key on [DepartmentId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuoteDepartment]
	FOREIGN KEY ([DepartmentId])
	REFERENCES [dbo].[Departments]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteDepartment'
CREATE INDEX [IX_FK_QuoteDepartment]
ON [dbo].[Quotes]
	([DepartmentId]);
GO

-- Creating foreign key on [QuoteId] in table 'FollowUps'
ALTER TABLE [dbo].[FollowUps]
ADD CONSTRAINT [FK_QuoteFollowUp]
	FOREIGN KEY ([QuoteId])
	REFERENCES [dbo].[Quotes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteFollowUp'
CREATE INDEX [IX_FK_QuoteFollowUp]
ON [dbo].[FollowUps]
	([QuoteId]);
GO



-- Creating foreign key on [PackageType1Id] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuotePackageType]
	FOREIGN KEY ([PackageType1Id])
	REFERENCES [dbo].[PackageTypes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuotePackageType'
CREATE INDEX [IX_FK_QuotePackageType]
ON [dbo].[Quotes]
	([PackageType1Id]);
GO

-- Creating foreign key on [PackageType2Id] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_PackageType2QuotePackageType]
	FOREIGN KEY ([PackageType2Id])
	REFERENCES [dbo].[PackageTypes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PackageType2QuotePackageType'
CREATE INDEX [IX_FK_PackageType2QuotePackageType]
ON [dbo].[Quotes]
	([PackageType2Id]);
GO

-- Creating foreign key on [PackageType3Id] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_PackageType3QuotePackageType]
	FOREIGN KEY ([PackageType3Id])
	REFERENCES [dbo].[PackageTypes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PackageType3QuotePackageType'
CREATE INDEX [IX_FK_PackageType3QuotePackageType]
ON [dbo].[Quotes]
	([PackageType3Id]);
GO

-- Creating foreign key on [PackageType4Id] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_PackageType4QuotePackageType]
	FOREIGN KEY ([PackageType4Id])
	REFERENCES [dbo].[PackageTypes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PackageType4QuotePackageType'
CREATE INDEX [IX_FK_PackageType4QuotePackageType]
ON [dbo].[Quotes]
	([PackageType4Id]);
GO

-- Creating foreign key on [PackageType5Id] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_PackageType5QuotePackageType]
	FOREIGN KEY ([PackageType5Id])
	REFERENCES [dbo].[PackageTypes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PackageType5QuotePackageType'
CREATE INDEX [IX_FK_PackageType5QuotePackageType]
ON [dbo].[Quotes]
	([PackageType5Id]);
GO

-- Creating foreign key on [QuoteTypeCode] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuoteQuoteType]
	FOREIGN KEY ([QuoteTypeCode])
	REFERENCES [dbo].[QuoteTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteQuoteType'
CREATE INDEX [IX_FK_QuoteQuoteType]
ON [dbo].[Quotes]
	([QuoteTypeCode]);
GO

-- Creating foreign key on [CostMeasurementId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_CostQuoteChargesMeasurement]
	FOREIGN KEY ([CostMeasurementId])
	REFERENCES [dbo].[Measurements]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CostQuoteChargesMeasurement'
CREATE INDEX [IX_FK_CostQuoteChargesMeasurement]
ON [dbo].[QuoteCharges]
	([CostMeasurementId]);
GO

-- Creating foreign key on [ContainerType1MarkUpTypeCode] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_ContainerType1QuoteChargesMarkUpType]
	FOREIGN KEY ([ContainerType1MarkUpTypeCode])
	REFERENCES [dbo].[MarkUpTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContainerType1QuoteChargesMarkUpType'
CREATE INDEX [IX_FK_ContainerType1QuoteChargesMarkUpType]
ON [dbo].[QuoteCharges]
	([ContainerType1MarkUpTypeCode]);
GO

-- Creating foreign key on [ContainerType2MarkUpTypeCode] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_ContainerType2QuoteChargesMarkUpType]
	FOREIGN KEY ([ContainerType2MarkUpTypeCode])
	REFERENCES [dbo].[MarkUpTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContainerType2QuoteChargesMarkUpType'
CREATE INDEX [IX_FK_ContainerType2QuoteChargesMarkUpType]
ON [dbo].[QuoteCharges]
	([ContainerType2MarkUpTypeCode]);
GO

-- Creating foreign key on [ContainerType3MarkUpTypeCode] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_ContainerType3QuoteChargesMarkUpType]
	FOREIGN KEY ([ContainerType3MarkUpTypeCode])
	REFERENCES [dbo].[MarkUpTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContainerType3QuoteChargesMarkUpType'
CREATE INDEX [IX_FK_ContainerType3QuoteChargesMarkUpType]
ON [dbo].[QuoteCharges]
	([ContainerType3MarkUpTypeCode]);
GO

-- Creating foreign key on [ContainerType4MarkUpTypeCode] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_ContainerType4QuoteChargesMarkUpType]
	FOREIGN KEY ([ContainerType4MarkUpTypeCode])
	REFERENCES [dbo].[MarkUpTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContainerType4QuoteChargesMarkUpType'
CREATE INDEX [IX_FK_ContainerType4QuoteChargesMarkUpType]
ON [dbo].[QuoteCharges]
	([ContainerType4MarkUpTypeCode]);
GO

-- Creating foreign key on [ContainerType5MarkUpTypeCode] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_ContainerType5QuoteChargesMarkUpType]
	FOREIGN KEY ([ContainerType5MarkUpTypeCode])
	REFERENCES [dbo].[MarkUpTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContainerType5QuoteChargesMarkUpType'
CREATE INDEX [IX_FK_ContainerType5QuoteChargesMarkUpType]
ON [dbo].[QuoteCharges]
	([ContainerType5MarkUpTypeCode]);
GO

-- Creating foreign key on [MarkUpTypeCode] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_QuoteChargesMarkUpType]
	FOREIGN KEY ([MarkUpTypeCode])
	REFERENCES [dbo].[MarkUpTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteChargesMarkUpType'
CREATE INDEX [IX_FK_QuoteChargesMarkUpType]
ON [dbo].[QuoteCharges]
	([MarkUpTypeCode]);
GO

-- Creating foreign key on [VendorId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_VendorQuoteChargesCard]
	FOREIGN KEY ([VendorId])
	REFERENCES [dbo].[Cards]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_VendorQuoteChargesCard'
CREATE INDEX [IX_FK_VendorQuoteChargesCard]
ON [dbo].[QuoteCharges]
	([VendorId]);
GO


-- Creating foreign key on [MainCarriageCarrierId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_MaincarriageQuoteCard]
	FOREIGN KEY ([MainCarriageCarrierId])
	REFERENCES [dbo].[Cards]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MaincarriageQuoteCard'
CREATE INDEX [IX_FK_MaincarriageQuoteCard]
ON [dbo].[Quotes]
	([MainCarriageCarrierId]);
GO

-- Creating foreign key on [QuoteId] in table 'QuotePriceSteps'
ALTER TABLE [dbo].[QuotePriceSteps]
ADD CONSTRAINT [FK_QuotePriceStepsQuote]
	FOREIGN KEY ([QuoteId])
	REFERENCES [dbo].[Quotes]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuotePriceStepsQuote'
CREATE INDEX [IX_FK_QuotePriceStepsQuote]
ON [dbo].[QuotePriceSteps]
	([QuoteId]);
GO

-- Creating foreign key on [CustomerId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_CustomerCardQuote]
	FOREIGN KEY ([CustomerId])
	REFERENCES [dbo].[Cards]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCardQuote'
CREATE INDEX [IX_FK_CustomerCardQuote]
ON [dbo].[Quotes]
	([CustomerId]);
GO

-- Creating foreign key on [CustomerAddressId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_CustomerAddressQuote]
	FOREIGN KEY ([CustomerAddressId])
	REFERENCES [dbo].[Addresses]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerAddressQuote'
CREATE INDEX [IX_FK_CustomerAddressQuote]
ON [dbo].[Quotes]
	([CustomerAddressId]);
GO

-- Creating foreign key on [CustomerContactId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_CustomerContactQuote]
	FOREIGN KEY ([CustomerContactId])
	REFERENCES [dbo].[Contacts]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContactQuote'
CREATE INDEX [IX_FK_CustomerContactQuote]
ON [dbo].[Quotes]
	([CustomerContactId]);
GO

-- Creating foreign key on [QuoteCustomerTypeCode] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuoteQuoteCustomerType]
	FOREIGN KEY ([QuoteCustomerTypeCode])
	REFERENCES [dbo].[QuoteCustomerTypes]
		([Code])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteQuoteCustomerType'
CREATE INDEX [IX_FK_QuoteQuoteCustomerType]
ON [dbo].[Quotes]
	([QuoteCustomerTypeCode]);
GO

-- Creating foreign key on [PotentialCustomerId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuotePotentialCutomer]
	FOREIGN KEY ([PotentialCustomerId])
	REFERENCES [dbo].[PotentialCustomers]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuotePotentialCutomer'
CREATE INDEX [IX_FK_QuotePotentialCutomer]
ON [dbo].[Quotes]
	([PotentialCustomerId]);
GO



-- Creating foreign key on [SaleCurrencyId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_QuoteCurrency]
	FOREIGN KEY ([SaleCurrencyId])
	REFERENCES [dbo].[Currencies]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuoteCurrency'
CREATE INDEX [IX_FK_QuoteCurrency]
ON [dbo].[Quotes]
	([SaleCurrencyId]);
GO

-- Creating foreign key on [CostCurrencyId] in table 'QuoteCharges'
ALTER TABLE [dbo].[QuoteCharges]
ADD CONSTRAINT [FK_CurrencyQuoteCharge]
	FOREIGN KEY ([CostCurrencyId])
	REFERENCES [dbo].[Currencies]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrencyQuoteCharge'
CREATE INDEX [IX_FK_CurrencyQuoteCharge]
ON [dbo].[QuoteCharges]
	([CostCurrencyId]);
GO

-- Creating foreign key on [PotentialShipperId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_PotentialShipperQuote]
	FOREIGN KEY ([PotentialShipperId])
	REFERENCES [dbo].[PotentialCustomers]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PotentialShipperQuote'
CREATE INDEX [IX_FK_PotentialShipperQuote]
ON [dbo].[Quotes]
	([PotentialShipperId]);
GO

-- Creating foreign key on [PotentialConsigneeId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_PotentialCustomerQuote]
	FOREIGN KEY ([PotentialConsigneeId])
	REFERENCES [dbo].[PotentialCustomers]
		([Id])
	ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PotentialCustomerQuote'
CREATE INDEX [IX_FK_PotentialCustomerQuote]
ON [dbo].[Quotes]
	([PotentialConsigneeId]);
GO


-- Creating foreign key on [FromAddressId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_FromQuoteAddress]
    FOREIGN KEY ([FromAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FromQuoteAddress'
CREATE INDEX [IX_FK_FromQuoteAddress]
ON [dbo].[Quotes]
    ([FromAddressId]);
GO

-- Creating foreign key on [ToAddressId] in table 'Quotes'
ALTER TABLE [dbo].[Quotes]
ADD CONSTRAINT [FK_ToQuoteAddress]
    FOREIGN KEY ([ToAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ToQuoteAddress'
CREATE INDEX [IX_FK_ToQuoteAddress]
ON [dbo].[Quotes]
    ([ToAddressId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------