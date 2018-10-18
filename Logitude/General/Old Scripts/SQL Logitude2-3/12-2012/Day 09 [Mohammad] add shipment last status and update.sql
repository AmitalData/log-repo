alter table shipments add ShipmentLastStatusCode varchar(4) null
go

alter table shipments add ShipmentLastStatusUpdate DateTime null
go

-- Creating foreign key on [ShipmentLastStatusCode] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_ShipmentAWBStatus]
    FOREIGN KEY ([ShipmentLastStatusCode])
    REFERENCES [dbo].[AWBStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentAWBStatus'
CREATE INDEX [IX_FK_ShipmentAWBStatus]
ON [dbo].[Shipments]
    ([ShipmentLastStatusCode]);
GO