-- Create New Table With Name CargoTrackingShipments
CREATE TABLE [dbo].[CargoTrackingShipments](
[Id] INT IDENTITY(1,1) NOT NULL,
[Tenant] INT NOT NULL,
[EntityId] VARCHAR(16) NULL,
[ForwardingShipmentHeaderId] VARCHAR(15) NULL,
[CustomsShipmentHeaderId] VARCHAR(15) NULL,
[EntityType] VARCHAR(1) NULL,
[CurrentMilestoneCode] VARCHAR(2) NULL,
[CurrentMilestoneDate] DATETIME NULL,
[CustomerId] VARCHAR(15) NULL,
[TransportModeId] VARCHAR(15) NULL,
[Master] VARCHAR(20) NULL,
[House] VARCHAR(20) NULL,
[ShipmentNumber] VARCHAR(20) NULL,
[FromPortId] VARCHAR(15) NULL,
[ToPortId] VARCHAR(15) NULL,
[ShipperId] VARCHAR(15) NULL,
[ConsigneeId] VARCHAR(15) NULL,
[GrossWeight] FLOAT NULL,
[Volume] FLOAT NULL,
[PickupDone] BIT NULL,
[PickupDate] DATETIME NULL,
[CreateDate] DATETIME NOT NULL,
[SecurityKey] VARCHAR(40) NULL,
[ConsigneeName] VARCHAR(70) NULL,
[ShipperName] VARCHAR(70) NULL,
[CustomerReference] VARCHAR(101) NULL,
[IsMainRecord] BIT DEFAULT(0) NOT NULL,
[PickupEstimationDate] DATETIME NULL,
[FromWarehouseDate] DATETIME NULL,
[FromWarehouseEstimationDate] DATETIME NULL,
[FromWarehouseNotes] NVARCHAR(32) NULL,
[DepartureDone] BIT NULL,
[DepartureDate] DATETIME NULL,
[DepartureEstimationDate] DATETIME NULL,
[ArrivalDone] BIT NULL,
[ArrivalDate] DATETIME NULL,
[ArrivalEstimationDate] DATETIME NULL,
[ToWarehouseDone] BIT NULL,
[ToWarehouseDate] DATETIME NULL,
[ToWarehouseEstimationDate] DATETIME NULL,
[ToWarehouseNotes] NVARCHAR(32) NULL,
[CustomsPaymentDone] BIT NULL,
[CustomsPaymentDate] DATETIME NULL,
[ClearanceDone] BIT NULL,
[ClearanceDate] DATETIME NULL,
[DeliveredDone] BIT NULL,
[DeliveredDate] DATETIME NULL,
[DeliveredEstimationDate] DATETIME NULL,
[FromWarehouseDone] BIT NULL,
[FirstPickupETD] DATETIME NULL,
[WarehouseLegActualEntryDate] DATETIME NULL,
[WarehouseLegExpectedEntryDate] DATETIME NULL,
[WarehouseLegRemarks] NVARCHAR(500) NULL,
[DeclarationDate] DATETIME NULL,
[CustomsClearanceDate] DATETIME NULL,
CONSTRAINT [PK_CargoTrackingShipments] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ab21f66c-6282-4999-bb69-f7545da9f7d3', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CargoTrackingShipmentsCREATE TABLE [dbo].[CargoTrackingShipments]([Id] INT IDENTITY(1,1) NOT NULL,[Tenant] INT NOT NULL,[EntityId] VARCHAR(16) NULL,[ForwardingShipmentHeaderId] VARCHAR(15) NULL,[CustomsShipmentHeaderId] VARCHAR(15) NULL,[EntityType] VARCHAR(1) NULL,[CurrentMilestoneCode] VARCHAR(2) NULL,[CurrentMilestoneDate] DATETIME NULL,[CustomerId] VARCHAR(15) NULL,[TransportModeId] VARCHAR(15) NULL,[Master] VARCHAR(20) NULL,[House] VARCHAR(20) NULL,[ShipmentNumber] VARCHAR(20) NULL,[FromPortId] VARCHAR(15) NULL,[ToPortId] VARCHAR(15) NULL,[ShipperId] VARCHAR(15) NULL,[ConsigneeId] VARCHAR(15) NULL,[GrossWeight] FLOAT NULL,[Volume] FLOAT NULL,[PickupDone] BIT NULL,[PickupDate] DATETIME NULL,[CreateDate] DATETIME NOT NULL,[SecurityKey] VARCHAR(40) NULL,[ConsigneeName] VARCHAR(70) NULL,[ShipperName] VARCHAR(70) NULL,[CustomerReference] VARCHAR(101) NULL,[IsMainRecord] BIT DEFAULT(0) NOT NULL,[PickupEstimationDate] DATETIME NULL,[FromWarehouseDate] DATETIME NULL,[FromWarehouseEstimationDate] DATETIME NULL,[FromWarehouseNotes] NVARCHAR(32) NULL,[DepartureDone] BIT NULL,[DepartureDate] DATETIME NULL,[DepartureEstimationDate] DATETIME NULL,[ArrivalDone] BIT NULL,[ArrivalDate] DATETIME NULL,[ArrivalEstimationDate] DATETIME NULL,[ToWarehouseDone] BIT NULL,[ToWarehouseDate] DATETIME NULL,[ToWarehouseEstimationDate] DATETIME NULL,[ToWarehouseNotes] NVARCHAR(32) NULL,[CustomsPaymentDone] BIT NULL,[CustomsPaymentDate] DATETIME NULL,[ClearanceDone] BIT NULL,[ClearanceDate] DATETIME NULL,[DeliveredDone] BIT NULL,[DeliveredDate] DATETIME NULL,[DeliveredEstimationDate] DATETIME NULL,[FromWarehouseDone] BIT NULL,[FirstPickupETD] DATETIME NULL,[WarehouseLegActualEntryDate] DATETIME NULL,[WarehouseLegExpectedEntryDate] DATETIME NULL,[WarehouseLegRemarks] NVARCHAR(500) NULL,[DeclarationDate] DATETIME NULL,[CustomsClearanceDate] DATETIME NULL,CONSTRAINT [PK_CargoTrackingShipments] PRIMARY KEY([Id]));');

-- Create Unique Constraint On CargoTrackingShipments Table
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f448ca3d-1086-45cd-8ba5-01f25e011840', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType,EntityId,Tenant', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On CargoTrackingShipments TableEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])'');');


-- Drop Primary Key Constraint
EXEC('IF (OBJECT_ID(''[dbo].[PK_CargoTrackingShipmentMasters]'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipmentMasters] DROP CONSTRAINT [PK_CargoTrackingShipmentMasters] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e82810e8-bdaf-44f1-ba93-e86debcbc27c', 'CargoTrackingShipmentMaster.dxml', 'CargoTrackingShipmentMasters', NULL, 'Drop Primary Key', GETDATE(), '-- Drop Primary Key ConstraintEXEC(''IF (OBJECT_ID(''''[dbo].[PK_CargoTrackingShipmentMasters]'''', ''''PK'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipmentMasters] DROP CONSTRAINT [PK_CargoTrackingShipmentMasters] END'');');

-- Change Size From 16 To 15 For Column Id
ALTER TABLE [dbo].[CargoTrackingShipmentMasters] ALTER COLUMN [Id] VARCHAR(15) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('90101172-4b8b-4b96-8f0d-13b4308a4e93', 'CargoTrackingShipmentMaster.dxml', 'CargoTrackingShipmentMasters', 'Id', 'Alter Column Size', GETDATE(), '-- Change Size From 16 To 15 For Column IdALTER TABLE [dbo].[CargoTrackingShipmentMasters] ALTER COLUMN [Id] VARCHAR(15) NOT NULL;');

-- Add Primary Key Constraint
EXEC('ALTER TABLE [dbo].[CargoTrackingShipmentMasters] ADD CONSTRAINT [PK_CargoTrackingShipmentMasters] PRIMARY KEY ([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bf49cf0c-9cea-42ff-ab51-16e57dd406df', 'CargoTrackingShipmentMaster.dxml', 'CargoTrackingShipmentMasters', 'Id', 'Add Primary Key', GETDATE(), '-- Add Primary Key ConstraintEXEC(''ALTER TABLE [dbo].[CargoTrackingShipmentMasters] ADD CONSTRAINT [PK_CargoTrackingShipmentMasters] PRIMARY KEY ([Id])'');');


-- Add Foreign Key Constraint For Column EntityType In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingHeaderEntityTypes
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('aca8574f-1acd-4f23-9ee0-78b5b2e1029b', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column EntityType In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingHeaderEntityTypesEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])'');');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_EntityType] ON [dbo].[CargoTrackingShipments]([EntityType])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0a371e03-ff5d-4ed5-b639-6d0729396045', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_EntityType] ON [dbo].[CargoTrackingShipments]([EntityType])'');');

-- Add Foreign Key Constraint For Column CurrentMilestoneCode In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingMilestones
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8a09e039-24a6-4e2b-923b-0db8ac3d442f', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CurrentMilestoneCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrentMilestoneCode In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingMilestonesEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])'');');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CurrentMilestoneCode] ON [dbo].[CargoTrackingShipments]([CurrentMilestoneCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('db0d1838-3d7f-42a7-9025-a214e5640692', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CurrentMilestoneCode', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CurrentMilestoneCode] ON [dbo].[CargoTrackingShipments]([CurrentMilestoneCode])'');');


