
--alter table shipmentpackages drop column FreightAmount
--alter table shipmentpackages drop column ChargeableWeight
--alter table shipmentpackages drop column AWBRateCharge
--alter table shipmentpackages drop column CommodityNumber

--alter table shipmentpackages drop [FK_ShipmentPackageRateClass]
--drop INDEX shipmentpackages.[IX_FK_ShipmentPackageRateClass]


---- Creating table 'ShipmentCommodities'
--CREATE TABLE [dbo].[ShipmentCommodities] (
--    [Id]varchar(15)   NOT NULL,
--    [Tenant]int   NOT NULL,
--    [ChargeableWeight]float   NULL,
--    [ChargeRate]float   NULL,
--    [ChargeAmount]float   NULL,
--    [CommodityNumber]varchar(7)   NULL,
--    [ShipmentId]varchar(15)   NOT NULL,
--    [RateClassCode]varchar(3)   NULL
--);
--GO


---- Creating primary key on [Id] in table 'ShipmentCommodities'
--ALTER TABLE [dbo].[ShipmentCommodities]
--ADD CONSTRAINT [PK_ShipmentCommodities]
--    PRIMARY KEY CLUSTERED ([Id] ASC);
--GO



---- Creating foreign key on [ShipmentId] in table 'ShipmentCommodities'
--ALTER TABLE [dbo].[ShipmentCommodities]
--ADD CONSTRAINT [FK_ShipmentShipmentCommodity]
--    FOREIGN KEY ([ShipmentId])
--    REFERENCES [dbo].[Shipments]
--        ([Id])
--    ON DELETE NO ACTION ON UPDATE NO ACTION;

---- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentShipmentCommodity'
--CREATE INDEX [IX_FK_ShipmentShipmentCommodity]
--ON [dbo].[ShipmentCommodities]
--    ([ShipmentId]);
--GO

--alter table shipmentpackages add ShipmentCommodityId varchar(15) null

---- Creating foreign key on [ShipmentCommodityId] in table 'ShipmentPackages'
--ALTER TABLE [dbo].[ShipmentPackages]
--ADD CONSTRAINT [FK_ShipmentCommodityShipmentPackage]
--    FOREIGN KEY ([ShipmentCommodityId])
--    REFERENCES [dbo].[ShipmentCommodities]
--        ([Id])
--    ON DELETE NO ACTION ON UPDATE NO ACTION;

---- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentCommodityShipmentPackage'
--CREATE INDEX [IX_FK_ShipmentCommodityShipmentPackage]
--ON [dbo].[ShipmentPackages]
--    ([ShipmentCommodityId]);
--GO


----alter table ShipmentCommodities add RateClassCode varchar(3) null

---- Creating foreign key on [RateClassCode] in table 'ShipmentCommodities'
--ALTER TABLE [dbo].[ShipmentCommodities]
--ADD CONSTRAINT [FK_ShipmentCommodityRateClass]
--    FOREIGN KEY ([RateClassCode])
--    REFERENCES [dbo].[RateClasses]
--        ([Code])
--    ON DELETE NO ACTION ON UPDATE NO ACTION;

---- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentCommodityRateClass'
--CREATE INDEX [IX_FK_ShipmentCommodityRateClass]
--ON [dbo].[ShipmentCommodities]
--    ([RateClassCode]);
--GO


