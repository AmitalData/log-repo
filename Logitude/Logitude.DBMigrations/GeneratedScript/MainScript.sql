-- Unset Nullable For Column Drop_HasPickup
ALTER TABLE [dbo].[ChargesTypes] ALTER COLUMN [Drop_HasPickup] BIT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('49918af4-f19e-48d3-b56c-f5ad644eedd1', 'ChargesType.dxml', 'ChargesTypes', 'Drop_HasPickup', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column Drop_HasPickupALTER TABLE [dbo].[ChargesTypes] ALTER COLUMN [Drop_HasPickup] BIT NOT NULL;');

-- Rename Column From Drop_HasPickup To HasPickup
EXEC SP_RENAME 'dbo.ChargesTypes.Drop_HasPickup', 'HasPickup', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('17b9a87a-9102-4668-8707-658854ca4f17', 'ChargesType.dxml', 'ChargesTypes', 'Drop_HasPickup', 'Rename Column', GETDATE(), '-- Rename Column From Drop_HasPickup To HasPickupEXEC SP_RENAME ''dbo.ChargesTypes.Drop_HasPickup'', ''HasPickup'', ''COLUMN'';');

-- Unset Nullable For Column Drop_HasDelivery
ALTER TABLE [dbo].[ChargesTypes] ALTER COLUMN [Drop_HasDelivery] BIT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4eeafbac-c372-40f8-9024-525ad736204f', 'ChargesType.dxml', 'ChargesTypes', 'Drop_HasDelivery', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column Drop_HasDeliveryALTER TABLE [dbo].[ChargesTypes] ALTER COLUMN [Drop_HasDelivery] BIT NOT NULL;');

-- Rename Column From Drop_HasDelivery To HasDelivery
EXEC SP_RENAME 'dbo.ChargesTypes.Drop_HasDelivery', 'HasDelivery', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('858baced-2b45-4e78-bbd7-9a536c2f8aa5', 'ChargesType.dxml', 'ChargesTypes', 'Drop_HasDelivery', 'Rename Column', GETDATE(), '-- Rename Column From Drop_HasDelivery To HasDeliveryEXEC SP_RENAME ''dbo.ChargesTypes.Drop_HasDelivery'', ''HasDelivery'', ''COLUMN'';');


