-- Add New Column With Name ShipmentId
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [ShipmentId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1c808d6d-3df9-4c0f-885f-5ad0de8351ff', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'ShipmentId', 'Add Column', GETDATE(), '-- Add New Column With Name ShipmentIdALTER TABLE [dbo].[CargoTrackingShipmentSearches] ADD [ShipmentId] VARCHAR(15) NULL;');

-- Drop Column SecurityKey
EXEC SP_RENAME 'dbo.CargoTrackingShipmentSearches.SecurityKey', 'Drop_SecurityKey', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('973243f9-d1cb-4db1-83cb-84361deb2ff0', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'SecurityKey', 'Drop Column', GETDATE(), '-- Drop Column SecurityKeyEXEC SP_RENAME ''dbo.CargoTrackingShipmentSearches.SecurityKey'', ''Drop_SecurityKey'', ''COLUMN'';');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f5608327-3e12-41dd-882a-a09db402cedf', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'Tenant,SearchFields', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields])'');');


