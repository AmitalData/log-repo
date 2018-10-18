alter table shipmentcarrierstatuses
add Location varchar(15) null
go

-- Creating foreign key on [Location] in table 'ShipmentCarrierStatuses'
ALTER TABLE [dbo].[ShipmentCarrierStatuses]
ADD CONSTRAINT [FK_LocationShipmentCarrierStatusPort]
    FOREIGN KEY ([Location])
    REFERENCES [dbo].[Ports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LocationShipmentCarrierStatusPort'
CREATE INDEX [IX_FK_LocationShipmentCarrierStatusPort]
ON [dbo].[ShipmentCarrierStatuses]
    ([Location]);
GO