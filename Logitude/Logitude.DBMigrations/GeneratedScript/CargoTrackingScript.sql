-- Add New Column With Name CustomerReference
ALTER TABLE [dbo].[CargoTrackingShipments] ADD [CustomerReference] VARCHAR(101) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ab32542c-bb91-435e-985d-d4c65d59c515', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CustomerReference', 'Add Column', GETDATE(), '-- Add New Column With Name CustomerReferenceALTER TABLE [dbo].[CargoTrackingShipments] ADD [CustomerReference] VARCHAR(101) NULL;');


