-- Add New Column With Name HashCode
ALTER TABLE [dbo].[QueueMessages] ADD [HashCode] NVARCHAR(MAX) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e599a7a1-c8ff-4718-b6e1-fec6c0256ef6', 'QueueMessage.dxml', 'QueueMessages', 'HashCode', 'Add Column', GETDATE(), '-- Add New Column With Name HashCodeALTER TABLE [dbo].[QueueMessages] ADD [HashCode] NVARCHAR(MAX) NULL;');


