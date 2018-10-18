-- Creating table 'AWBChargesCodes'
CREATE TABLE [dbo].[AWBChargesCodes] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(100)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'AWBChargesCodes'
ALTER TABLE [dbo].[AWBChargesCodes]
ADD CONSTRAINT [PK_AWBChargesCodes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

alter table shipments
add AWBChargesCodeCode varchar(4) null

-- Creating foreign key on [AWBChargesCodeCode] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_AWBChargesCodeShipment]
    FOREIGN KEY ([AWBChargesCodeCode])
    REFERENCES [dbo].[AWBChargesCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AWBChargesCodeShipment'
CREATE INDEX [IX_FK_AWBChargesCodeShipment]
ON [dbo].[Shipments]
    ([AWBChargesCodeCode]);
GO

--***************** No need online ************
alter table tenants
add PackageCode varchar(4) null

-- Creating foreign key on [PackageCode] in table 'Tenants'
ALTER TABLE [dbo].[Tenants]
ADD CONSTRAINT [FK_TenantPackage]
    FOREIGN KEY ([PackageCode])
    REFERENCES [dbo].[Packages]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TenantPackage'
CREATE INDEX [IX_FK_TenantPackage]
ON [dbo].[Tenants]
    ([PackageCode]);
GO
--***************** No need online ************
alter table shipments 
add IssuingCarrierAgentId varchar(15) null

alter table shipments 
add IssuingCarrierAddressId varchar(15) null

-- Creating foreign key on [IssuingCarrierAgentId] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_IssuingCarrierShipmentCard]
    FOREIGN KEY ([IssuingCarrierAgentId])
    REFERENCES [dbo].[Cards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_IssuingCarrierShipmentCard'
CREATE INDEX [IX_FK_IssuingCarrierShipmentCard]
ON [dbo].[Shipments]
    ([IssuingCarrierAgentId]);
GO

-- Creating foreign key on [IssuingCarrierAddressId] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_IssuingCarrierShipmentAddress]
    FOREIGN KEY ([IssuingCarrierAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_IssuingCarrierShipmentAddress'
CREATE INDEX [IX_FK_IssuingCarrierShipmentAddress]
ON [dbo].[Shipments]
    ([IssuingCarrierAddressId]);
GO