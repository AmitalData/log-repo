

-- Creating table 'AWBStatus'
CREATE TABLE [dbo].[AWBStatus] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating table 'ShipmentCarrierStatuses'
CREATE TABLE [dbo].[ShipmentCarrierStatuses] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [ShipmentId]varchar(15)   NOT NULL,
    [Status]varchar(4)   NOT NULL,
    [ReceivingDate]datetime   NOT NULL,
    [Details]nvarchar(250)   NULL,
    [FromPortId]varchar(15)   NULL,
    [ToPortId]varchar(15)   NULL,
    [FlightNumber]varchar(15)   NOT NULL,
    [Pieces]int   NOT NULL,
    [Partial]bit   NOT NULL,
    [Weight]float   NOT NULL,
    [RecordHash]nvarchar(500)   NOT NULL,
    [EventDate]datetime   NOT NULL
);
GO

-- Creating primary key on [Code] in table 'AWBStatus'
ALTER TABLE [dbo].[AWBStatus]
ADD CONSTRAINT [PK_AWBStatus]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Id] in table 'ShipmentCarrierStatuses'
ALTER TABLE [dbo].[ShipmentCarrierStatuses]
ADD CONSTRAINT [PK_ShipmentCarrierStatuses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [Status] in table 'ShipmentCarrierStatuses'
ALTER TABLE [dbo].[ShipmentCarrierStatuses]
ADD CONSTRAINT [FK_ShipmentCarrierStatusAWBStatus]
    FOREIGN KEY ([Status])
    REFERENCES [dbo].[AWBStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentCarrierStatusAWBStatus'
CREATE INDEX [IX_FK_ShipmentCarrierStatusAWBStatus]
ON [dbo].[ShipmentCarrierStatuses]
    ([Status]);
GO

-- Creating foreign key on [FromPortId] in table 'ShipmentCarrierStatuses'
ALTER TABLE [dbo].[ShipmentCarrierStatuses]
ADD CONSTRAINT [FK_FromShipmentCarrierStatusPort]
    FOREIGN KEY ([FromPortId])
    REFERENCES [dbo].[Ports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FromShipmentCarrierStatusPort'
CREATE INDEX [IX_FK_FromShipmentCarrierStatusPort]
ON [dbo].[ShipmentCarrierStatuses]
    ([FromPortId]);
GO

-- Creating foreign key on [ToPortId] in table 'ShipmentCarrierStatuses'
ALTER TABLE [dbo].[ShipmentCarrierStatuses]
ADD CONSTRAINT [FK_ToShipmentCarrierStatusPort]
    FOREIGN KEY ([ToPortId])
    REFERENCES [dbo].[Ports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ToShipmentCarrierStatusPort'
CREATE INDEX [IX_FK_ToShipmentCarrierStatusPort]
ON [dbo].[ShipmentCarrierStatuses]
    ([ToPortId]);
GO
