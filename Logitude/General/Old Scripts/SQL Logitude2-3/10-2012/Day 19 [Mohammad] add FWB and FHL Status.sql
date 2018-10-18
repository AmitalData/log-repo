
-- Creating table 'FWBStatus'
CREATE TABLE [dbo].[FWBStatus] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating table 'FHLStatus'
CREATE TABLE [dbo].[FHLStatus] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'FWBStatus'
ALTER TABLE [dbo].[FWBStatus]
ADD CONSTRAINT [PK_FWBStatus]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'FHLStatus'
ALTER TABLE [dbo].[FHLStatus]
ADD CONSTRAINT [PK_FHLStatus]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

alter table Shipmentmasterdatas add FWBStatusCode varchar(4) null

alter table Shipments add FHLStatusCode varchar(4) null

-- Creating foreign key on [FWBStatusCode] in table 'ShipmentMasterDatas'
ALTER TABLE [dbo].[ShipmentMasterDatas]
ADD CONSTRAINT [FK_ShipmentMasterDataFWBStatus]
    FOREIGN KEY ([FWBStatusCode])
    REFERENCES [dbo].[FWBStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentMasterDataFWBStatus'
CREATE INDEX [IX_FK_ShipmentMasterDataFWBStatus]
ON [dbo].[ShipmentMasterDatas]
    ([FWBStatusCode]);
GO

-- Creating foreign key on [FHLStatusCode] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_ShipmentFHLStatus]
    FOREIGN KEY ([FHLStatusCode])
    REFERENCES [dbo].[FHLStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentFHLStatus'
CREATE INDEX [IX_FK_ShipmentFHLStatus]
ON [dbo].[Shipments]
    ([FHLStatusCode]);
GO