-- Set Nullable For Column ShipmentDate
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [ShipmentDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f351eeae-f2ac-4e3b-adf5-0e363f9f9cf5', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'ShipmentDate', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ShipmentDateALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [ShipmentDate] DATETIME NULL;');


