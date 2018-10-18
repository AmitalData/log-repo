

SET QUOTED_IDENTIFIER OFF;
GO
USE [WebFreightBranch_Design];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO



-- Creating table 'ShipmentPickUps'
CREATE TABLE [dbo].[ShipmentPickUps] (
    [Id] varchar(15)  NOT NULL,
    [Tenant] int  NOT NULL,
    [ShipmentId] varchar(15)  NOT NULL,
    [PickUpFromAddressId] varchar(15)  NOT NULL,
    [PickUpATD] datetime  NULL,
    [PickUpATA] datetime  NULL,
    [PickUpETD] datetime  NULL,
    [PickUpETA] datetime  NULL,
    [PickUpCarrierId] varchar(15)  NULL,
    [PickUpCarrierNumber] varchar(15)  NULL,
    [Notes] nvarchar(250)  NULL
);
GO

-- Creating table 'ShipmentDeliveries'
CREATE TABLE [dbo].[ShipmentDeliveries] (
    [Id] varchar(15)  NOT NULL,
    [Tenant] int  NOT NULL,
    [ShipmentId] varchar(15)  NOT NULL,
    [DeliveryToAddressId] varchar(15)  NOT NULL,
    [DeliveryATD] datetime  NULL,
    [DeliveryATA] datetime  NULL,
    [DeliveryETD] datetime  NULL,
    [DeliveryETA] datetime  NULL,
    [DeliveryCarrierId] varchar(15)  NULL,
    [DeliveryCarrierNumber] varchar(15)  NULL,
    [Notes] nvarchar(250)  NULL
);
GO


-- Creating primary key on [Id] in table 'ShipmentPickUps'
ALTER TABLE [dbo].[ShipmentPickUps]
ADD CONSTRAINT [PK_ShipmentPickUps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ShipmentDeliveries'
ALTER TABLE [dbo].[ShipmentDeliveries]
ADD CONSTRAINT [PK_ShipmentDeliveries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [ShipmentId] in table 'ShipmentPickUps'
ALTER TABLE [dbo].[ShipmentPickUps]
ADD CONSTRAINT [FK_ShipmentPickUpShipment]
    FOREIGN KEY ([ShipmentId])
    REFERENCES [dbo].[Shipments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentPickUpShipment'
CREATE INDEX [IX_FK_ShipmentPickUpShipment]
ON [dbo].[ShipmentPickUps]
    ([ShipmentId]);
GO

-- Creating foreign key on [PickUpFromAddressId] in table 'ShipmentPickUps'
ALTER TABLE [dbo].[ShipmentPickUps]
ADD CONSTRAINT [FK_ShipmentPickUpAddress]
    FOREIGN KEY ([PickUpFromAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentPickUpAddress'
CREATE INDEX [IX_FK_ShipmentPickUpAddress]
ON [dbo].[ShipmentPickUps]
    ([PickUpFromAddressId]);
GO

-- Creating foreign key on [PickUpCarrierId] in table 'ShipmentPickUps'
ALTER TABLE [dbo].[ShipmentPickUps]
ADD CONSTRAINT [FK_ShipmentPickUpCarrier]
    FOREIGN KEY ([PickUpCarrierId])
    REFERENCES [dbo].[Carriers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentPickUpCarrier'
CREATE INDEX [IX_FK_ShipmentPickUpCarrier]
ON [dbo].[ShipmentPickUps]
    ([PickUpCarrierId]);
GO

-- Creating foreign key on [ShipmentId] in table 'ShipmentDeliveries'
ALTER TABLE [dbo].[ShipmentDeliveries]
ADD CONSTRAINT [FK_ShipmentDeliveryShipment]
    FOREIGN KEY ([ShipmentId])
    REFERENCES [dbo].[Shipments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentDeliveryShipment'
CREATE INDEX [IX_FK_ShipmentDeliveryShipment]
ON [dbo].[ShipmentDeliveries]
    ([ShipmentId]);
GO

-- Creating foreign key on [DeliveryToAddressId] in table 'ShipmentDeliveries'
ALTER TABLE [dbo].[ShipmentDeliveries]
ADD CONSTRAINT [FK_ShipmentDeliveryAddress]
    FOREIGN KEY ([DeliveryToAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentDeliveryAddress'
CREATE INDEX [IX_FK_ShipmentDeliveryAddress]
ON [dbo].[ShipmentDeliveries]
    ([DeliveryToAddressId]);
GO

-- Creating foreign key on [DeliveryCarrierId] in table 'ShipmentDeliveries'
ALTER TABLE [dbo].[ShipmentDeliveries]
ADD CONSTRAINT [FK_ShipmentDeliveryCarrier]
    FOREIGN KEY ([DeliveryCarrierId])
    REFERENCES [dbo].[Carriers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentDeliveryCarrier'
CREATE INDEX [IX_FK_ShipmentDeliveryCarrier]
ON [dbo].[ShipmentDeliveries]
    ([DeliveryCarrierId]);
GO