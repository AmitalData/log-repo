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
[ClearanceDone] BIT NULL,
[PickupDate] DATETIME NULL,
[ClearanceDate] DATETIME NULL,
[CreateDate] DATETIME NOT NULL,
[SecurityKey] VARCHAR(40) NULL,
[ConsigneeName] VARCHAR(70) NULL,
[ShipperName] VARCHAR(70) NULL,
[CustomerReference] VARCHAR(101) NULL,
CONSTRAINT [PK_CargoTrackingShipments] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9ef1a063-4c99-4cdc-bf07-8b0e152e9a55', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CargoTrackingShipmentsCREATE TABLE [dbo].[CargoTrackingShipments]([Id] INT IDENTITY(1,1) NOT NULL,[Tenant] INT NOT NULL,[EntityId] VARCHAR(16) NULL,[ForwardingShipmentHeaderId] VARCHAR(15) NULL,[CustomsShipmentHeaderId] VARCHAR(15) NULL,[EntityType] VARCHAR(1) NULL,[CurrentMilestoneCode] VARCHAR(2) NULL,[CurrentMilestoneDate] DATETIME NULL,[CustomerId] VARCHAR(15) NULL,[TransportModeId] VARCHAR(15) NULL,[Master] VARCHAR(20) NULL,[House] VARCHAR(20) NULL,[ShipmentNumber] VARCHAR(20) NULL,[FromPortId] VARCHAR(15) NULL,[ToPortId] VARCHAR(15) NULL,[ShipperId] VARCHAR(15) NULL,[ConsigneeId] VARCHAR(15) NULL,[GrossWeight] FLOAT NULL,[Volume] FLOAT NULL,[PickupDone] BIT NULL,[ClearanceDone] BIT NULL,[PickupDate] DATETIME NULL,[ClearanceDate] DATETIME NULL,[CreateDate] DATETIME NOT NULL,[SecurityKey] VARCHAR(40) NULL,[ConsigneeName] VARCHAR(70) NULL,[ShipperName] VARCHAR(70) NULL,[CustomerReference] VARCHAR(101) NULL,CONSTRAINT [PK_CargoTrackingShipments] PRIMARY KEY([Id]));');

-- Create Unique Constraint On CargoTrackingShipments Table
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('68c2352f-d329-4a2d-b296-46113b4948ad', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType,EntityId,Tenant', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On CargoTrackingShipments TableEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])'');');


-- Drop Index IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields From Table CargoTrackingShipmentSearches
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields'' AND object_id = OBJECT_ID(''[dbo].[CargoTrackingShipmentSearches]'', ''U'')) BEGIN DROP INDEX [IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields] ON [dbo].[CargoTrackingShipmentSearches] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0855b224-a4f8-46e6-aa07-a633c4f5a1d4', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields From Table CargoTrackingShipmentSearchesEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields'''' AND object_id = OBJECT_ID(''''[dbo].[CargoTrackingShipmentSearches]'''', ''''U'''')) BEGIN DROP INDEX [IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields] ON [dbo].[CargoTrackingShipmentSearches] END'');');

-- Change Size From 101 To 100 For Column SearchFields
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [SearchFields] NVARCHAR(100);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('895d1ad3-4420-4692-98d3-c5e966188ba8', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'SearchFields', 'Alter Column Size', GETDATE(), '-- Change Size From 101 To 100 For Column SearchFieldsALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [SearchFields] NVARCHAR(100);');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[ShipmentDate],[SearchFields])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c8f41596-23b4-4f20-827c-bfb7d523b858', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'Tenant,ShipmentDate,SearchFields', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[ShipmentDate],[SearchFields])'');');


-- Add Foreign Key Constraint For Column EntityType In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingHeaderEntityTypes
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('60022f5a-5741-44d7-b0b8-c4c0bdad3e5c', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column EntityType In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingHeaderEntityTypesEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])'');');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_EntityType] ON [dbo].[CargoTrackingShipments]([EntityType])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('106017d5-244b-41bb-adab-6397e2897e9b', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_EntityType] ON [dbo].[CargoTrackingShipments]([EntityType])'');');

-- Add Foreign Key Constraint For Column CurrentMilestoneCode In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingMilestones
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5a2ca4e6-7921-44fb-812b-ca6716f6e9aa', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CurrentMilestoneCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrentMilestoneCode In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingMilestonesEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])'');');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CurrentMilestoneCode] ON [dbo].[CargoTrackingShipments]([CurrentMilestoneCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7e9464d2-035f-431a-affa-2f483242c68a', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CurrentMilestoneCode', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CurrentMilestoneCode] ON [dbo].[CargoTrackingShipments]([CurrentMilestoneCode])'');');


