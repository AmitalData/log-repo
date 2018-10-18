
IF OBJECT_ID(N'[dbo].[ShipmentPackageItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].ShipmentPackageItems;
GO

-- Creating table 'ShipmentPackageItems'
CREATE TABLE [dbo].ShipmentPackageItems (
    [PackageId]varchar(15) NOT NULL,
	[LineNumber]int NOT NULL,
	[Tenant]int NOT NULL,
	[Description]varchar(100) NOT NULL,
    [Quantity]int NOT NULL,
);
GO

-- Creating primary key on [PackageId],[LineNumber] in table 'ShipmentPackageItems'
ALTER TABLE [dbo].[ShipmentPackageItems]
ADD CONSTRAINT [PK_ShipmentPackageItems]
    PRIMARY KEY CLUSTERED ([PackageId],[LineNumber] ASC);
GO

-- Creating foreign key on [PackageId] in table 'ShipmentPackageItems'
ALTER TABLE [dbo].[ShipmentPackageItems]
ADD CONSTRAINT [FK_ShipmentPackageItemShipmentPackage]
    FOREIGN KEY ([PackageId])
    REFERENCES [dbo].[ShipmentPackages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
go