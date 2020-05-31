-- Unset Nullable For Column Drop_EnableMessageHash
ALTER TABLE [dbo].[QueueDefinitions] ALTER COLUMN [Drop_EnableMessageHash] BIT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e2f43f70-afd6-4275-96da-f5a96852892b', 'QueueDefinition.dxml', 'QueueDefinitions', 'Drop_EnableMessageHash', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column Drop_EnableMessageHashALTER TABLE [dbo].[QueueDefinitions] ALTER COLUMN [Drop_EnableMessageHash] BIT NOT NULL;');

-- Rename Column From Drop_EnableMessageHash To EnableMessageHash
EXEC SP_RENAME 'dbo.QueueDefinitions.Drop_EnableMessageHash', 'EnableMessageHash', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7d19c5b1-ca40-4807-8f55-78e9782792a9', 'QueueDefinition.dxml', 'QueueDefinitions', 'Drop_EnableMessageHash', 'Rename Column', GETDATE(), '-- Rename Column From Drop_EnableMessageHash To EnableMessageHashEXEC SP_RENAME ''dbo.QueueDefinitions.Drop_EnableMessageHash'', ''EnableMessageHash'', ''COLUMN'';');


