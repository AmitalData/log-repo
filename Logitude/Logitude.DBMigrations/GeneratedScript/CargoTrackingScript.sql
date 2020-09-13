-- Drop Default Value For Column IsPublic
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__IsPub__47DBAE45]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipmentSearches] DROP CONSTRAINT [DF__CargoTrac__IsPub__47DBAE45] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0ed51882-82fb-491d-8b4b-6233195a1cf5', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'IsPublic', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column IsPublicEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__IsPub__47DBAE45]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipmentSearches] DROP CONSTRAINT [DF__CargoTrac__IsPub__47DBAE45] END'');');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('805b7017-89c7-4d68-9b6b-7c3ceedf16aa', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'Tenant,SearchFields,IsPublic', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])'');');


