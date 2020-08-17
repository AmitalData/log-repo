-- Add New Column With Name SecurityKey
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [SecurityKey] VARCHAR(40) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bdbb2539-0c30-4358-9718-be71af3be6bf', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'SecurityKey', 'Add Column', GETDATE(), '-- Add New Column With Name SecurityKeyALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [SecurityKey] VARCHAR(40) NULL;');

-- Set Nullable For Column ShipmentId
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [ShipmentId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('525302ba-3f7e-4838-b996-da64de396f65', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'ShipmentId', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ShipmentIdALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [ShipmentId] VARCHAR(15) NULL;');

-- Drop Column ShipmentId
EXEC SP_RENAME 'dbo.CargoTrackingShipmentSearches.ShipmentId', 'Drop_ShipmentId', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('213a2695-1c05-4a75-a824-8538f8930187', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'ShipmentId', 'Drop Column', GETDATE(), '-- Drop Column ShipmentIdEXEC SP_RENAME ''dbo.CargoTrackingShipmentSearches.ShipmentId'', ''Drop_ShipmentId'', ''COLUMN'';');


