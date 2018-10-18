alter table shipmentReceivables
add IATACodeCode varchar(5) null

alter table shipmentPayables
add IATACodeCode varchar(5) null

alter table shipmentAWBPrintOnlies
add IATACodeCode varchar(5) null

alter table shipmentAWBPrintOnlies
add MeasurementId varchar(15) null

-- Creating foreign key on [IATACodeCode] in table 'ShipmentPayables'
ALTER TABLE [dbo].[ShipmentPayables]
ADD CONSTRAINT [FK_ShipmentPayableIATACode]
    FOREIGN KEY ([IATACodeCode])
    REFERENCES [dbo].[IATACodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentPayableIATACode'
CREATE INDEX [IX_FK_ShipmentPayableIATACode]
ON [dbo].[ShipmentPayables]
    ([IATACodeCode]);
GO

-- Creating foreign key on [IATACodeCode] in table 'ShipmentAWBPrintOnlies'
ALTER TABLE [dbo].[ShipmentAWBPrintOnlies]
ADD CONSTRAINT [FK_ShipmentAWBPrintOnlyIATACode]
    FOREIGN KEY ([IATACodeCode])
    REFERENCES [dbo].[IATACodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentAWBPrintOnlyIATACode'
CREATE INDEX [IX_FK_ShipmentAWBPrintOnlyIATACode]
ON [dbo].[ShipmentAWBPrintOnlies]
    ([IATACodeCode]);
GO

-- Creating foreign key on [MeasurementId] in table 'ShipmentAWBPrintOnlies'
ALTER TABLE [dbo].[ShipmentAWBPrintOnlies]
ADD CONSTRAINT [FK_ShipmentAWBPrintOnlyMeasurement]
    FOREIGN KEY ([MeasurementId])
    REFERENCES [dbo].[Measurements]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentAWBPrintOnlyMeasurement'
CREATE INDEX [IX_FK_ShipmentAWBPrintOnlyMeasurement]
ON [dbo].[ShipmentAWBPrintOnlies]
    ([MeasurementId]);
GO

-- Creating foreign key on [IATACodeCode] in table 'ShipmentReceivables'
ALTER TABLE [dbo].[ShipmentReceivables]
ADD CONSTRAINT [FK_ShipmentReceivableIATACode]
    FOREIGN KEY ([IATACodeCode])
    REFERENCES [dbo].[IATACodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentReceivableIATACode'
CREATE INDEX [IX_FK_ShipmentReceivableIATACode]
ON [dbo].[ShipmentReceivables]
    ([IATACodeCode]);
GO