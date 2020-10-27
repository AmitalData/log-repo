-- Drop Default Value For Column PickupDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__Picku__1C3D2329]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Picku__1C3D2329] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9fd15616-4d16-4200-afc4-07e9681ca9c1', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'PickupDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column PickupDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__Picku__1C3D2329]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Picku__1C3D2329] END'');');

-- Drop Default Value For Column DepartureDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__Depar__1E256B9B]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Depar__1E256B9B] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6e5751fd-0499-49cc-8e0b-44401c30fa49', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'DepartureDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column DepartureDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__Depar__1E256B9B]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Depar__1E256B9B] END'');');

-- Drop Default Value For Column ArrivalDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__Arriv__1F198FD4]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Arriv__1F198FD4] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('93b62867-fe09-4ee6-afaf-155b56d33938', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ArrivalDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column ArrivalDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__Arriv__1F198FD4]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Arriv__1F198FD4] END'');');

-- Drop Default Value For Column ToWarehouseDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__ToWar__200DB40D]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__ToWar__200DB40D] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c246debc-c4b6-4ee1-b414-42c4ed8c2f35', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ToWarehouseDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column ToWarehouseDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__ToWar__200DB40D]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__ToWar__200DB40D] END'');');

-- Change Size From 32 To 500 For Column ToWarehouseNotes
ALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [ToWarehouseNotes] NVARCHAR(500);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('89f265e9-a805-4eea-a35f-82bc1250441e', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ToWarehouseNotes', 'Alter Column Size', GETDATE(), '-- Change Size From 32 To 500 For Column ToWarehouseNotesALTER TABLE [dbo].[CargoTrackingShipments] ALTER COLUMN [ToWarehouseNotes] NVARCHAR(500);');

-- Drop Default Value For Column CustomsPaymentDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__Custo__2101D846]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Custo__2101D846] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('45e86762-7de5-4b6e-9cd7-28a11ddea5c0', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CustomsPaymentDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column CustomsPaymentDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__Custo__2101D846]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Custo__2101D846] END'');');

-- Drop Default Value For Column ClearanceDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__Clear__21F5FC7F]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Clear__21F5FC7F] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a24005f6-d1b3-449f-97fe-cdbcc2afb9f0', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ClearanceDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column ClearanceDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__Clear__21F5FC7F]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Clear__21F5FC7F] END'');');

-- Drop Default Value For Column DeliveredDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__Deliv__22EA20B8]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Deliv__22EA20B8] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6425cc90-fa7c-4dcf-ae16-0afb2a4542c6', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'DeliveredDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column DeliveredDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__Deliv__22EA20B8]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__Deliv__22EA20B8] END'');');

-- Drop Default Value For Column FromWarehouseDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__FromW__23DE44F1]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__FromW__23DE44F1] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a4208aed-be74-42a6-a73b-0fe553a5078e', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'FromWarehouseDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column FromWarehouseDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__FromW__23DE44F1]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__Pre_Cargo__FromW__23DE44F1] END'');');


-- Drop Index IX_Pre_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic From Table CargoTrackingShipmentSearches
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_Pre_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic'' AND object_id = OBJECT_ID(''[dbo].[CargoTrackingShipmentSearches]'', ''U'')) BEGIN DROP INDEX [IX_Pre_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e5581876-65d1-41e5-b5c7-9c36de35a62f', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_Pre_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic From Table CargoTrackingShipmentSearchesEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_Pre_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic'''' AND object_id = OBJECT_ID(''''[dbo].[CargoTrackingShipmentSearches]'''', ''''U'''')) BEGIN DROP INDEX [IX_Pre_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches] END'');');

-- Change Size From 100 To 1000 For Column SearchFields
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [SearchFields] NVARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0fcb0d13-d73f-446b-96cc-cab4ab052f66', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'SearchFields', 'Alter Column Size', GETDATE(), '-- Change Size From 100 To 1000 For Column SearchFieldsALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [SearchFields] NVARCHAR(1000);');

-- Drop Default Value For Column IsPublic
EXEC('IF (OBJECT_ID(''[dbo].[DF__Pre_Cargo__IsPub__26BAB19C]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipmentSearches] DROP CONSTRAINT [DF__Pre_Cargo__IsPub__26BAB19C] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1652678c-4f6a-4526-bc18-cd8080a75254', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'IsPublic', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column IsPublicEXEC(''IF (OBJECT_ID(''''[dbo].[DF__Pre_Cargo__IsPub__26BAB19C]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipmentSearches] DROP CONSTRAINT [DF__Pre_Cargo__IsPub__26BAB19C] END'');');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d7565d95-5b19-49ee-93ee-0b284fc60ef6', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'Tenant,SearchFields,IsPublic', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_SearchFields_IsPublic] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[SearchFields],[IsPublic])'');');


