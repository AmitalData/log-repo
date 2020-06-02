-- Rename Column From Drop_CustomField To CustomField
EXEC SP_RENAME 'dbo.EventTypes.Drop_CustomField', 'CustomField', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9b7d58a8-4ca3-493c-8684-53b8fd5c174d', 'EventType.dxml', 'EventTypes', 'Drop_CustomField', 'Rename Column', GETDATE(), '-- Rename Column From Drop_CustomField To CustomFieldEXEC SP_RENAME ''dbo.EventTypes.Drop_CustomField'', ''CustomField'', ''COLUMN'';');


