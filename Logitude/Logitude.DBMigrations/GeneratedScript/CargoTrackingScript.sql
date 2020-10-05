-- Add New Column With Name ErrorLog
ALTER TABLE [dbo].[CargoTrackingIncrementalStats] ADD [ErrorLog] NVARCHAR(4000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3787e0be-7806-4222-9df7-782a73721deb', 'CargoTrackingIncrementalStat.dxml', 'CargoTrackingIncrementalStats', 'ErrorLog', 'Add Column', GETDATE(), '-- Add New Column With Name ErrorLogALTER TABLE [dbo].[CargoTrackingIncrementalStats] ADD [ErrorLog] NVARCHAR(4000) NULL;');


-- Change Size From 32 To 500 For Column FromWarehouseNotes
ALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [FromWarehouseNotes] NVARCHAR(500);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9e1cdbd9-9c1a-4063-802a-cded2920ca1d', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'FromWarehouseNotes', 'Alter Column Size', GETDATE(), '-- Change Size From 32 To 500 For Column FromWarehouseNotesALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [FromWarehouseNotes] NVARCHAR(500);');

-- Change Size From 32 To 500 For Column ToWarehouseNotes
ALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [ToWarehouseNotes] NVARCHAR(500);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b09a8c94-f402-4e90-b067-6ff2354eb3d8', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ToWarehouseNotes', 'Alter Column Size', GETDATE(), '-- Change Size From 32 To 500 For Column ToWarehouseNotesALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [ToWarehouseNotes] NVARCHAR(500);');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_Tenant_IsMainRecord_EntityId] ON [dbo].[CargoTrackingShipments]([Tenant],[IsMainRecord],[EntityId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c0732368-1144-4591-ba81-d4d499e1dfb5', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'Tenant,IsMainRecord,EntityId', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_Tenant_IsMainRecord_EntityId] ON [dbo].[CargoTrackingShipments]([Tenant],[IsMainRecord],[EntityId])'');');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_Tenant_IsMainRecord_EntityId_CustomsShipmentHeaderId] ON [dbo].[CargoTrackingShipments]([Tenant],[IsMainRecord],[EntityId],[CustomsShipmentHeaderId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('de8017a7-8291-4296-83c3-08519b833eb1', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'Tenant,IsMainRecord,EntityId,CustomsShipmentHeaderId', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_Tenant_IsMainRecord_EntityId_CustomsShipmentHeaderId] ON [dbo].[CargoTrackingShipments]([Tenant],[IsMainRecord],[EntityId],[CustomsShipmentHeaderId])'');');

-- Create Unique Constraint On CargoTrackingShipments Table
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4f6c1cf0-0b7e-45e9-a822-2e8cbfa5620f', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType,EntityId,Tenant', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On CargoTrackingShipments TableEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])'');');


-- Add New Column With Name ShipmentId
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [ShipmentId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('175658a5-b6d8-43a1-b48a-5f6679907ce9', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'ShipmentId', 'Add Column', GETDATE(), '-- Add New Column With Name ShipmentIdALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [ShipmentId] VARCHAR(15) NULL;');

-- Add New Column With Name IsPublic
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [IsPublic] BIT DEFAULT(0) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('20952373-4949-4cfc-8183-8178ed71e2ab', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'IsPublic', 'Add Column', GETDATE(), '-- Add New Column With Name IsPublicALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [IsPublic] BIT DEFAULT(0) NULL;');

-- Drop Column SecurityKey
EXEC SP_RENAME 'dbo.CargoTrackingShipmentSearches.SecurityKey', 'Drop_SecurityKey', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d310f92f-b9c5-4952-aac4-3aa15774bf28', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'SecurityKey', 'Drop Column', GETDATE(), '-- Drop Column SecurityKeyEXEC SP_RENAME ''dbo.CargoTrackingShipmentSearches.SecurityKey'', ''Drop_SecurityKey'', ''COLUMN'';');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('22bfd076-4379-4f74-a579-f85fe07dab6f', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'Tenant,SearchFields,IsPublic', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])'');');


