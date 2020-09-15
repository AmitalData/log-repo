-- Drop Default Value For Column PickupDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__Picku__403A8C7D]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Picku__403A8C7D] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('05bc611c-6d18-4490-a5d6-50ee0ec0707e', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'PickupDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column PickupDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__Picku__403A8C7D]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Picku__403A8C7D] END'');');

-- Drop Default Value For Column DepartureDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__Depar__4222D4EF]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Depar__4222D4EF] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('48ecb2dd-5977-4dc5-95ed-efc2fb1c49f2', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'DepartureDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column DepartureDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__Depar__4222D4EF]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Depar__4222D4EF] END'');');

-- Drop Default Value For Column ArrivalDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__Arriv__4316F928]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Arriv__4316F928] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('19a3caee-e4aa-4d72-ba36-0ba6737d0f03', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ArrivalDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column ArrivalDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__Arriv__4316F928]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Arriv__4316F928] END'');');

-- Drop Default Value For Column ToWarehouseDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__ToWar__440B1D61]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__ToWar__440B1D61] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('161c30ff-35bc-4e7b-a2ae-8df8fe2a78d0', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ToWarehouseDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column ToWarehouseDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__ToWar__440B1D61]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__ToWar__440B1D61] END'');');

-- Drop Default Value For Column CustomsPaymentDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__Custo__44FF419A]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Custo__44FF419A] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7075ffc3-8635-4a69-8a85-573e559bbcc1', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CustomsPaymentDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column CustomsPaymentDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__Custo__44FF419A]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Custo__44FF419A] END'');');

-- Drop Default Value For Column ClearanceDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__Clear__45F365D3]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Clear__45F365D3] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9085eb73-c0c3-48ee-867b-ddb13830ae4d', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'ClearanceDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column ClearanceDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__Clear__45F365D3]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Clear__45F365D3] END'');');

-- Drop Default Value For Column DeliveredDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__Deliv__46E78A0C]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Deliv__46E78A0C] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0508f55d-ba44-4192-a95e-23833d13b0b4', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'DeliveredDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column DeliveredDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__Deliv__46E78A0C]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__Deliv__46E78A0C] END'');');

-- Drop Default Value For Column FromWarehouseDone
EXEC('IF (OBJECT_ID(''[dbo].[DF__CargoTrac__FromW__47DBAE45]'', ''D'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__FromW__47DBAE45] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('21becfc7-d10e-4a29-b9a2-0f71706026a8', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'FromWarehouseDone', 'Drop Default Value', GETDATE(), '-- Drop Default Value For Column FromWarehouseDoneEXEC(''IF (OBJECT_ID(''''[dbo].[DF__CargoTrac__FromW__47DBAE45]'''', ''''D'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [DF__CargoTrac__FromW__47DBAE45] END'');');

-- Drop Unique Constraint UQ_CargoTrackingShipments_EntityType_EntityId_Tenant From Table CargoTrackingShipments
EXEC('IF (OBJECT_ID(''[dbo].[UQ_CargoTrackingShipments_EntityType_EntityId_Tenant]'', ''UQ'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c208d28f-5ee4-4086-a71f-10a529eed745', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', NULL, 'Drop Unique Constraint', GETDATE(), '-- Drop Unique Constraint UQ_CargoTrackingShipments_EntityType_EntityId_Tenant From Table CargoTrackingShipmentsEXEC(''IF (OBJECT_ID(''''[dbo].[UQ_CargoTrackingShipments_EntityType_EntityId_Tenant]'''', ''''UQ'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[CargoTrackingShipments] DROP CONSTRAINT [UQ_CargoTrackingShipments_EntityType_EntityId_Tenant] END'');');


