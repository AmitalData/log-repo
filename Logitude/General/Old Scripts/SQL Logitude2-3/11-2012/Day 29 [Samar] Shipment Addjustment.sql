 --Shipment addjustment
  
 alter table ShipmentMasterDatas add [MainCarriageFromPartnerId] varchar(15)  NULL
 alter table ShipmentMasterDatas add [MainCarriageToPartnerId] varchar(15)  NULL
 alter table ShipmentMasterDatas add [MainCarriageFromAddressId] varchar(15)  NULL
 alter table ShipmentMasterDatas add [MainCarriageToAddressId] varchar(15)  NULL
 alter table ShipmentMasterDatas add [Driver] nvarchar(40)  NULL
 alter table ShipmentMasterDatas add [TruckNumber] nvarchar(15)  NULL
 alter table ShipmentMasterDatas add [TrailerNumber] nvarchar(15)  NULL
 

-- Creating foreign key on [MainCarriageFromPartnerId] in table 'ShipmentMasterDatas'
ALTER TABLE [dbo].[ShipmentMasterDatas]
ADD CONSTRAINT [FK_FromPartnerCardShipmentMasterData]
    FOREIGN KEY ([MainCarriageFromPartnerId])
    REFERENCES [dbo].[Cards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FromPartnerCardShipmentMasterData'
CREATE INDEX [IX_FK_FromPartnerCardShipmentMasterData]
ON [dbo].[ShipmentMasterDatas]
    ([MainCarriageFromPartnerId]);
GO


-- Creating foreign key on [MainCarriageToPartnerId] in table 'ShipmentMasterDatas'
ALTER TABLE [dbo].[ShipmentMasterDatas]
ADD CONSTRAINT [FK_ToPartnerCardShipmentMasterData]
    FOREIGN KEY ([MainCarriageToPartnerId])
    REFERENCES [dbo].[Cards]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ToPartnerCardShipmentMasterData'
CREATE INDEX [IX_FK_ToPartnerCardShipmentMasterData]
ON [dbo].[ShipmentMasterDatas]
    ([MainCarriageToPartnerId]);
GO


-- Creating foreign key on [MainCarriageFromAddressId] in table 'ShipmentMasterDatas'
ALTER TABLE [dbo].[ShipmentMasterDatas]
ADD CONSTRAINT [FK_FromPartnerAddressShipmentMasterData]
    FOREIGN KEY ([MainCarriageFromAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FromPartnerAddressShipmentMasterData'
CREATE INDEX [IX_FK_FromPartnerAddressShipmentMasterData]
ON [dbo].[ShipmentMasterDatas]
    ([MainCarriageFromAddressId]);
GO


-- Creating foreign key on [MainCarriageToAddressId] in table 'ShipmentMasterDatas'
ALTER TABLE [dbo].[ShipmentMasterDatas]
ADD CONSTRAINT [FK_ToPartnerAddressShipmentMasterData]
    FOREIGN KEY ([MainCarriageToAddressId])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ToPartnerAddressShipmentMasterData'
CREATE INDEX [IX_FK_ToPartnerAddressShipmentMasterData]
ON [dbo].[ShipmentMasterDatas]
    ([MainCarriageToAddressId]);
GO